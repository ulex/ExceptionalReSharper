/// <copyright>Copyright (c) 2009 CodeGears.net All rights reserved.</copyright>

using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Daemon.CSharp.Stages;

namespace CodeGears.ReSharper.Exceptional.Highlightings
{
    /// <summary>Base class for all highlightings.</summary>
    /// <remarks>Provides default implementation.</remarks>
    public abstract class HighlightingBase : CSharpHighlightingBase, IHighlighting
    {
        public override bool IsValid()
        {
            return true;
        }

        public virtual string ToolTip
        {
            get { return Message; }
        }

        public virtual string ErrorStripeToolTip
        {
            get { return Message; }
        }

        protected abstract string Message { get; }

        public virtual int NavigationOffsetPatch
        {
            get { return 0; }
        }
    }
}