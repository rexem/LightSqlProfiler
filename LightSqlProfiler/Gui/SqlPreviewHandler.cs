using ICSharpCode.AvalonEdit.Folding;
using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;
using LightSqlProfiler.Models.Config;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;

namespace LightSqlProfiler.Gui
{
    /// <summary>
    /// SQL code preview control handler
    /// </summary>
    public class SqlPreviewHandler : ObservableObject
    {
        private ICSharpCode.AvalonEdit.TextEditor _editor;

        private EditorSettings _settings;

        public SqlPreviewHandler(ICSharpCode.AvalonEdit.TextEditor editor, EditorSettings settings)
        {
            _editor = editor;
            _settings = settings;
        }

        /// <summary>
        /// Sets content
        /// </summary>
        /// <param name="text"></param>
        public void SetText(string text)
        {
            _editor.Text = text;
        }

        public string GetText()
        {
            return _editor.Text;
        }

        /// <summary>
        /// Prepares preview control by registering syntax highlighting and foldings
        /// </summary>
        public void Setup()
        {
            IHighlightingDefinition customHighlighting;
            string resourceName = Assembly.GetExecutingAssembly().GetManifestResourceNames().Single(str => str.EndsWith("sql.xshd", StringComparison.InvariantCultureIgnoreCase));
            using (Stream s = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName))
            {
                if (s == null)
                    throw new InvalidOperationException("Could not find embedded resource");
                using (XmlReader reader = new XmlTextReader(s))
                {
                    customHighlighting = HighlightingLoader.Load(reader, HighlightingManager.Instance);
                }
            }
            // and register it in the HighlightingManager
            HighlightingManager.Instance.RegisterHighlighting("Custom Highlighting", new string[] { ".cool" }, customHighlighting);

            _editor.SyntaxHighlighting = customHighlighting;

            // folding
            var mgr = FoldingManager.Install(_editor.TextArea);
            var strat = new XmlFoldingStrategy();
            strat.UpdateFoldings(mgr, _editor.Document);
        }

        /// <summary>
        /// Copies all contents to the clipboard
        /// </summary>
        public void CopyToClipboard()
        {
            System.Windows.Clipboard.SetText(_editor.Text);
        }
    }
}
