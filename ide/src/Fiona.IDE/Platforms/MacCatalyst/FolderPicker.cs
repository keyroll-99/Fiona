using Fiona.IDE.Components.Layout.Menu;
using Foundation;
using UIKit;

namespace Fiona.IDE.Platforms.MacCatalyst
{
    public class FolderPicker : IFolderPicker
    {
        private class PickerDelegate : UIDocumentPickerDelegate
        {
            public required Action<NSUrl[]> PickHandler { get; init; }

            public override void WasCancelled(UIDocumentPickerViewController controller)
                => PickHandler?.Invoke(null);

            public override void DidPickDocument(UIDocumentPickerViewController controller, NSUrl[] urls)
                => PickHandler?.Invoke(urls);

            public override void DidPickDocument(UIDocumentPickerViewController controller, NSUrl url)
                => PickHandler?.Invoke([url]);
        }

        private static void GetFileResults(IReadOnlyList<NSUrl> urls, TaskCompletionSource<string> tcs)
        {
            try
            {
                tcs.TrySetResult(urls?[0]?.ToString() ?? "");
            }
            catch (Exception ex)
            {
                tcs.TrySetException(ex);
            }
        }

        public async Task<string> PickFolder()
        {
            string[] allowedTypes = ["public.folder"];

            UIDocumentPickerViewController picker = new(allowedTypes, UIDocumentPickerMode.Open);
            TaskCompletionSource<string> tcs = new();

            picker.Delegate = new PickerDelegate { PickHandler = urls => GetFileResults(urls, tcs) };

            if (picker.PresentationController != null)
            {
                picker.PresentationController.Delegate =
                    new UiPresentationControllerDelegate(() => GetFileResults(null, tcs));
            }

            UIViewController? parentController = Platform.GetCurrentUIViewController();

            parentController.PresentViewController(picker, true, null);

            return await tcs.Task;
        }

        internal class UiPresentationControllerDelegate : UIAdaptivePresentationControllerDelegate
        {
            private Action _dismissHandler;

            internal UiPresentationControllerDelegate(Action dismissHandler)
                => this._dismissHandler = dismissHandler;

            public override void DidDismiss(UIPresentationController presentationController)
            {
                _dismissHandler?.Invoke();
                _dismissHandler = null;
            }

            protected override void Dispose(bool disposing)
            {
                _dismissHandler?.Invoke();
                base.Dispose(disposing);
            }
        }
    }
}