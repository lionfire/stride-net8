using Stride.Core;
using Stride.Core.Mathematics;
using Stride.Engine.Design;
using Stride.Engine.Processors;

namespace Stride.Engine.Splines.Components
{
    /// <summary>
    /// Component representing a Spline node.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Associate this component to an entity to maintain bezier curves that together form a spline.
    /// </para>
    /// </remarks>

    [DataContract]
    [DefaultEntityComponentProcessor(typeof(SplineNodeTransformProcessor), ExecutionMode = ExecutionMode.All)]
    [Display("Spline node", Expand = ExpandRule.Once)]
    [ComponentCategory("Splines")]
    public sealed class SplineNodeComponent : EntityComponent
    {
        #region Out
        private Vector3 _tangentOut { get; set; }
        public Vector3 TangentOut
        {
            get { return _tangentOut; }
            set
            {
                _tangentOut = value;
            }
        }
        #endregion

        #region In
        private Vector3 _tangentIn { get; set; }
        public Vector3 TangentIn

        {
            get { return _tangentIn; }
            set
            {
                _tangentIn = value;
            }
        }
        #endregion

        #region Segments
        private int _segments = 2;
        public int Segments
        {
            get { return _segments; }
            set
            {
                if (value < 2)
                {
                    _segments = 2;
                }
                else
                {
                    _segments = value;
                }
                _bezierCurve?.MakeDirty();
            }
        }
        #endregion

        private BezierCurve _bezierCurve;
        private Vector3 _previousPosition;

        internal void Update(TransformComponent transformComponent)
        {
            CheckDirtyness();

            _previousPosition = Entity.Transform.Position;
        }

        private void CheckDirtyness()
        {
            if (_bezierCurve != null && 
                ( _previousPosition.X != Entity.Transform.Position.X || _previousPosition.Y != Entity.Transform.Position.Y || _previousPosition.Z != Entity.Transform.Position.Z))
            {
                _bezierCurve?.MakeDirty();
            }
        }

        public void UpdateBezierCurve(SplineNodeComponent nextNode)
        {
            if (nextNode != null)
            {
                Vector3 scale;
                Quaternion rotation;
                Vector3 entityWorldPos;
                Vector3 nextWorldPos;

                Entity.Transform.WorldMatrix.Decompose(out scale, out rotation, out entityWorldPos);
                nextNode.Entity.Transform.WorldMatrix.Decompose(out scale, out rotation, out nextWorldPos);
                Vector3 TangentOutWorld = entityWorldPos + TangentOut;
                Vector3 TangentInWorld = nextWorldPos + nextNode.TangentIn;

                _bezierCurve = new BezierCurve(Segments, entityWorldPos, TangentOutWorld, nextWorldPos, TangentInWorld);
            }
        }

        public BezierCurve GetBezierCurve()
        {
            return _bezierCurve;
        }
    }
}
