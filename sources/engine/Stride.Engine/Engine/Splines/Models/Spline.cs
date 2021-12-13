using System.Collections.Generic;
using Stride.Core;

namespace Stride.Engine.Splines
{
    [DataContract]
    public partial class Spline
    {
        /// <summary>
        /// Event triggered when the spline has been update
        /// This happens when the entity is translated or rotated, or when a SplineNode is updated
        /// </summary>
        public delegate void SplineUpdatedHandler();
        public event SplineUpdatedHandler OnSplineUpdated;

        private List<SplineNode> splineNodes;
        private SplineDebugInfo debugInfo = new();

        [DataMemberIgnore]
        public List<SplineNode> SplineNodes
        {
            get
            {
                splineNodes ??= new List<SplineNode>();
                return splineNodes;
            }
            set
            {
                splineNodes = value;
                DeregisterSplineNodeDirtyEvents();
                RegisterSplineNodeDirtyEvents();

                Dirty = true;
            }
        }

        [DataMemberIgnore]
        public bool Dirty { get; set; }

        public float TotalSplineDistance { get; internal set; }

        private bool loop;
        /// <summary>
        /// The last spline node reconnects to the first spline node. This still requires a minimum of 2 spline nodes.
        /// </summary>
        [Display(60, "Loop")]
        public bool Loop
        {
            get
            {
                return loop;
            }
            set
            {
                loop = value;
                Dirty = true;
            }
        }

        [Display(80, "Debug settings")]
        public SplineDebugInfo DebugInfo
        {
            get
            {
                return debugInfo;
            }
            set
            {
                debugInfo = value;
                Dirty = true;
            }
        }

        /// <summary>
        /// Updates the splines and its splines nodes
        /// </summary>
        public void UpdateSpline()
        {
            if (Dirty)
            {
                var totalNodesCount = SplineNodes.Count;
                if (SplineNodes.Count > 1)
                {

                    for (int i = 0; i < totalNodesCount; i++)
                    {
                        var currentSplineNode = SplineNodes[i];
                        if (SplineNodes[i] == null)
                            break;

                        if (i < totalNodesCount - 1)
                        {
                            var nextSplineNode = SplineNodes[i + 1];
                            if (nextSplineNode == null)
                                break;

                            currentSplineNode.TargetWorldPosition = nextSplineNode.WorldPosition;
                            currentSplineNode.TargetTangentInWorldPosition = nextSplineNode.TangentInWorldPosition;

                            SplineNodes[i].CalculateBezierCurve();
                        }
                        else if (i == totalNodesCount - 1 && Loop)
                        {
                            var firstSplineNode = SplineNodes[0];
                            currentSplineNode.TargetWorldPosition = firstSplineNode.WorldPosition;
                            currentSplineNode.TargetTangentInWorldPosition = firstSplineNode.TangentInWorldPosition;

                            SplineNodes[i].CalculateBezierCurve();
                        }
                    }
                }

                TotalSplineDistance = GetTotalSplineDistance();

                OnSplineUpdated?.Invoke();
            }
        }

        /// <summary>
        /// Retrieves the total distance of the spline
        /// </summary>
        /// <returns>The total distance of the spline</returns>
        public float GetTotalSplineDistance()
        {
            float distance = 0;
            for (int i = 0; i < SplineNodes.Count; i++)
            {
                var curve = SplineNodes[i];
                if (curve != null)
                {
                    if (Loop || (!Loop && i < SplineNodes.Count - 1))
                    {
                        distance += curve.Distance;
                    }
                }
            }

            return distance;
        }

        /// <summary>
        /// Retrieve information of the spline position at give percentage
        /// </summary>
        /// <param name="percentage"></param>
        /// <returns>Various details on the specific part of the spline</returns>
        public SplinePositionInfo GetPositionOnSpline(float percentage)
        {
            var splinePositionInfo = new SplinePositionInfo();
            var totalSplineDistance = GetTotalSplineDistance();
            if (totalSplineDistance <= 0)
                return splinePositionInfo;

            var requiredDistance = totalSplineDistance * (percentage / 100);
            var nextNodeDistance = 0.0f;
            var prevNodeDistance = 0.0f;

            for (int i = 0; i < SplineNodes.Count; i++)
            {
                var currentSplineNode = SplineNodes[i];
                splinePositionInfo.CurrentSplineNodeIndex = i;
                splinePositionInfo.CurrentSplineNode = currentSplineNode;

                nextNodeDistance += currentSplineNode.Distance;

                if (requiredDistance < nextNodeDistance)
                {
                    var targetIndex = (i == splineNodes.Count - 1) ? 0 : i;
                    splinePositionInfo.TargetSplineNode = splineNodes[targetIndex];

                    // Inverse lerp(betweenValue - minHeight) / (maxHeight - minHeight);
                    var percentageInCurve = ((requiredDistance - prevNodeDistance) / (nextNodeDistance - prevNodeDistance)) * 100;

                    splinePositionInfo.Position = currentSplineNode.GetPositionOnCurve(percentageInCurve);
                    return splinePositionInfo;
                }

                prevNodeDistance = nextNodeDistance;
            }

            splinePositionInfo.Position = splineNodes[splineNodes.Count - 2].TargetWorldPosition;

            return splinePositionInfo;
        }

        public void DeregisterSplineNodeDirtyEvents()
        {
            for (int i = 0; i < SplineNodes?.Count; i++)
            {
                var splineNode = SplineNodes[i];
                if (splineNode != null)
                {
                    splineNode.OnSplineNodeDirty -= MakeSplineDirty;
                }
            }
        }

        public void RegisterSplineNodeDirtyEvents()
        {
            for (int i = 0; i < SplineNodes?.Count; i++)
            {
                var splineNode = SplineNodes[i];
                if (splineNode != null)
                {
                    splineNode.OnSplineNodeDirty += MakeSplineDirty;
                }
            }
        }

        private void MakeSplineDirty()
        {
            Dirty = true;
        }
    }
}
