namespace TextEditor
{
    using System;
    using BL;

    public class MainPresenter
    {
        private readonly IMainForm _view;
        private readonly IFileManager _manager;
        private readonly IMessageService _messageService;
        private string _currentFilePath;

        public MainPresenter(IMainForm view, IFileManager manager, IMessageService messageService)
        {
            _view = view;
            _manager = manager;
            _messageService = messageService;

            _view.SetSymbolCount(0);
            _view.ContentChanged += ViewOnContentChanged;
            _view.FileOpenClick += ViewOnFileOpenClick;
            _view.FileSaveClick += ViewOnFileSaveClick;
        }

        private void ViewOnFileSaveClick(object sender, EventArgs eventArgs)
        {
            try
            {
                string content = _view.Content;

                _manager.SaveContent(content, _currentFilePath);
                _messageService.ShowMessage("File successfully saved");
            }
            catch (Exception e)
            {
                _messageService.ShowError(e.Message);
            }
        }

        private void ViewOnFileOpenClick(object sender, EventArgs eventArgs)
        {
            try
            {
                string filePath = _view.FilePath;

                bool isExist = _manager.IsExist(filePath);

                if (!isExist)
                {
                    _messageService.ShowExclamation("File not found");
                    return;
                }

                _currentFilePath = filePath;

                string content = _manager.GetContent(filePath);
                int count = _manager.GetSymbolCount(content);

                _view.Content = content;
                _view.SetSymbolCount(count);
            }
            catch (Exception e)
            {
                _messageService.ShowError(e.Message);
            }
        }

        private void ViewOnContentChanged(object sender, EventArgs eventArgs)
        {
            string content = _view.Content;

            int count = _manager.GetSymbolCount(content);

            _view.SetSymbolCount(count);
        }
    }
}