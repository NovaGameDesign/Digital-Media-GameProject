using UnityEngine.UIElements;

namespace DigitalMedia
{
    public class SplitView : TwoPaneSplitView
    {
        public new class UxmlFacotry : UxmlFactory<SplitView, TwoPaneSplitView.UxmlTraits> {}
    }
}
