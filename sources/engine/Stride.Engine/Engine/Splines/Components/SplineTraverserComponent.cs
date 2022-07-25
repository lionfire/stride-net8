//// Copyright (c) Stride contributors (https://Stride.com)
//// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using Stride.Core;
using Stride.Engine.Design;
using Stride.Engine.Splines.Models;
using Stride.Engine.Splines.Processors;

namespace Stride.Engine.Splines.Components
{
    /// <summary>
    /// Component representing a Spline Traverser.
    /// </summary>
    [DataContract("SplineTraverserComponent")]
    [Display("Spline Traverser", Expand = ExpandRule.Once)]
    [DefaultEntityComponentProcessor(typeof(SplineTraverserTransformProcessor))]
    [ComponentCategory("Splines")]
    public sealed class SplineTraverserComponent : EntityComponent
    {
        private SplineTraverser splineTraverser;
        private SplineComponent splineComponent;

        /// <summary>
        /// SplineTraverser object
        /// </summary>
        [DataMemberIgnore]
        public SplineTraverser SplineTraverser
        {
            get {
                splineTraverser ??= new SplineTraverser();
                return splineTraverser; 
            }
            set
            {
                splineTraverser = value;
                SplineTraverser.EnqueueSplineTraverserUpdate();
            }
        }

        /// <summary>
        /// The spline to traverse
        /// No spline, no movement
        /// </summary>
        [Display(10, "Spline")]
        public SplineComponent Spline
        {
            get { return splineComponent; }
            set
            {
                splineComponent = value;

                if (splineComponent == null)
                {
                    IsMoving = false;
                }

                SplineTraverser.Spline = splineComponent?.Spline;
                SplineTraverser.Entity = Entity;
            }
        }

        /// <summary>
        /// The speed at which the traverser moves over the spline
        /// Use a negative value, to go in to the opposite direction
        /// Note: Using a high value, can cause jitters. With a higher speed value, it is recommended to reduced the amount of spline points
        /// </summary>
        [Display(20, "Speed")]
        public float Speed
        {
            get { return SplineTraverser.Speed; }
            set
            {
                SplineTraverser.Speed = value;

                if (SplineTraverser.Speed == 0)
                {
                    IsMoving = false;
                }
            }
        }

        /// <summary>
        /// For a traverse to work we require a Spline reference, a non-zero and IsMoving must be True
        /// </summary>
        [Display(40, "Moving")]
        public bool IsMoving
        {
            get
            {
                return SplineTraverser.IsMoving;
            }
            set
            {
                SplineTraverser.IsMoving = value;
            }
        }

        internal void Update(TransformComponent transformComponent)
        {

        }

        public void ActivateSplineNodeReached(SplineNodeComponent splineNode)
        {
             //OnSplineNodeReached?.Invoke(splineNode);
        }

        public void ActivateOnSplineEndReached()
        {
            //OnSplineEndReached?.Invoke();
        }
    }
}
