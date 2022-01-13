using Stride.Graphics;
using Stride.Rendering;
using Stride.Core;
using Stride.Core.Mathematics;
using Stride.Engine.Splines.Models.Mesh;

namespace Stride.Engine.Splines
{
    [DataContract]
    public class SplineRenderer
    {
        private Entity splineMeshEntity;
        private Entity segmentsEntity;
        private Entity boundingBoxEntity;
        private bool segments;
        private bool boundingBox;

        public delegate void SplineRendererSettingsUpdatedHandler();
        public event SplineRendererSettingsUpdatedHandler OnSplineRendererSettingsUpdated;

        /// <summary>
        /// Display spline curve mesh
        /// </summary>
        [Display(10, "Segments")]
        public bool Segments
        {
            get
            {
                return segments;
            }
            set
            {
                segments = value;

                OnSplineRendererSettingsUpdated?.Invoke();
            }
        }

        /// <summary>
        /// The material used by the spline mesh
        /// </summary>
        [Display(20, "Segments material")]
        public Material SegmentsMaterial;

        /// <summary>
        /// Display the bounding boxes of each node and the entire spline
        /// </summary>
        [Display(30, "Show boundingbox")]
        public bool BoundingBox
        {
            get { return boundingBox; }
            set
            {
                boundingBox = value;

                OnSplineRendererSettingsUpdated?.Invoke();
            }
        }

        /// <summary>
        /// The material used by the spline boundingboxes
        /// </summary>
        [Display(40, "Boundingbox material")]
        public Material BoundingBoxMaterial;

        /// <summary>
        /// Creates an entity with a mesh that visualises various spline parts like segments and bounding boxes
        /// </summary>
        /// <param name="spline"></param>
        /// <param name="graphicsDevice"></param>
        /// <param name="splinePosition"></param>
        /// <returns>An entity with sub entities containing various meshes to visualise the spline</returns>
        public Entity Create(Spline spline, GraphicsDevice graphicsDevice, Vector3 splinePosition)
        {
            //Create the entities that hold the various debug meshes           
            splineMeshEntity ??= new Entity("SplineRenderer");
            segmentsEntity ??= new Entity("Segments");
            boundingBoxEntity ??= new Entity("BoundingBox");

            if (segmentsEntity.GetParent() == null)
            {
                splineMeshEntity.AddChild(segmentsEntity);
            }
            if (boundingBoxEntity.GetParent() == null)
            {
                splineMeshEntity.AddChild(boundingBoxEntity);
            }

            ClearChildren(segmentsEntity);
            ClearChildren(boundingBoxEntity);

            if (graphicsDevice == null || SegmentsMaterial == null || spline == null)
                return splineMeshEntity;

            var nodes = spline.SplineNodes;

            if (nodes?.Count > 1)
            {
                var totalNodesCount = nodes.Count;
                for (int i = 0; i < totalNodesCount; i++)
                {
                    var currentSplineNode = nodes[i];

                    if (currentSplineNode == null)
                    {
                        break;
                    }

                    // Dont create a mesh when it is the last node and Loop is disabled
                    if (i == totalNodesCount - 1 && !spline.Loop) 
                    {
                        break;
                    }

                    if (Segments || BoundingBox)
                    {
                        if (currentSplineNode == null) return splineMeshEntity;

                        var curvePointsInfo = currentSplineNode.GetBezierPoints();

                        if (curvePointsInfo?[0] == null)
                            break;

                        var splinePoints = new Vector3[curvePointsInfo.Length];
                        for (int j = 0; j < curvePointsInfo.Length; j++)
                        {
                            if (curvePointsInfo[j] == null)
                                break;
                            splinePoints[j] = curvePointsInfo[j].Position;
                        }

                        if (Segments && SegmentsMaterial != null)
                        {
                            DrawSegmentLine(i.ToString(), splinePoints, graphicsDevice, splinePosition);
                        }

                        if (BoundingBox && BoundingBoxMaterial != null)
                        {
                            UpdateSegmentBoundingBox(i.ToString(), currentSplineNode.BoundingBox, graphicsDevice, splinePosition);
                        }
                    }
                }
            }

            if (BoundingBox && BoundingBoxMaterial != null)
            {
                UpdateSegmentBoundingBox("Spline", spline.BoundingBox, graphicsDevice, splinePosition);
            }

            return splineMeshEntity;
        }

        private void DrawSegmentLine(string description, Vector3[] splinePoints, GraphicsDevice graphicsDevice, Vector3 position)
        {
            var splineMeshData = new SplineMeshData(splinePoints, graphicsDevice);
            var segments = new Entity($"Segment_{description}") { new ModelComponent { Model = new Model { SegmentsMaterial, new Mesh { Draw = splineMeshData.Build() }, }, RenderGroup = RenderGroup.Group4, IsShadowCaster = false } };
            segmentsEntity.AddChild(segments);
            segments.Transform.Position -= position - segmentsEntity.Transform.Position;
        }

        private void UpdateSegmentBoundingBox(string description, BoundingBox boundingBox, GraphicsDevice graphicsDevice, Vector3 position)
        {
            var boundingBoxMesh = new BoundingBoxMesh(graphicsDevice);
            boundingBoxMesh.Build(boundingBox);

            var boundingBoxChild = new Entity($"BoundingBox_{description}") { new ModelComponent { Model = new Model { BoundingBoxMaterial ?? SegmentsMaterial, new Mesh { Draw = boundingBoxMesh.MeshDraw } }, RenderGroup = RenderGroup.Group4, IsShadowCaster = false } };
            boundingBoxEntity.AddChild(boundingBoxChild);
            boundingBoxChild.Transform.Position -= position - boundingBoxChild.Transform.Position;
        }

        private void ClearChildren(Entity entity)
        {
            var children = entity.GetChildren();
            foreach (var child in children)
            {
                entity?.RemoveChild(child);
            }
        }
    }
}
