using LightSqlProfiler.Core;
using LightSqlProfiler.Core.Trace.Definitions;
using LightSqlProfiler.Gui;
using LightSqlProfiler.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace LightSqlProfiler.ViewModels
{
    public class ColumnSelectorVM : ObservableObject
    {
        public Action CloseAction { get; set; }

        public delegate void OnSaveDelegate(IEnumerable<GuiGridColumn> result);

        private OnSaveDelegate _onSave;

        public ObservableCollection<GuiGridColumn> AvailableColumns { get; set; } = new ObservableCollection<GuiGridColumn>();

        private GuiGridColumn _selectedAvailableColumn;

        public GuiGridColumn SelectedAvailableColumn
        {
            get { return _selectedAvailableColumn; }
            set { _selectedAvailableColumn = value; OnPropertyChanged(); }
        }

        public ObservableCollection<GuiGridColumn> SelectedColumns { get; set; } = new ObservableCollection<GuiGridColumn>();

        private GuiGridColumn _selectedSelectedColumn;

        public GuiGridColumn SelectedSelectedColumn
        {
            get { return _selectedSelectedColumn; }
            set { _selectedSelectedColumn = value; OnPropertyChanged(); }
        }

        #region Commands

        public ICommand CloseCommand { get; }
        public ICommand SaveCommand { get; }
        public ICommand AddColumnCommand { get; }
        public ICommand RemoveColumnCommand { get; }
        public ICommand UpColumnCommand { get; }
        public ICommand DownColumnCommand { get; }
        public ICommand ResetDefaultsCommand { get; }

        #endregion Commands

        public ColumnSelectorVM(IEnumerable<GuiGridColumn> columns, OnSaveDelegate onSave)
        {
            _onSave = onSave;

            CloseCommand = new DelegateCommand(
                o => true,
                o => CloseAction()
            );

            SaveCommand = new DelegateCommand(
                o => true,
                o => Save()
            );

            UpColumnCommand = new DelegateCommand(
                o => SelectedSelectedColumn != null && SelectedColumns.IndexOf(SelectedSelectedColumn) > 0,
                o => UpColumn(SelectedSelectedColumn)
            );

            DownColumnCommand = new DelegateCommand(
                o => SelectedSelectedColumn != null && SelectedColumns.IndexOf(SelectedSelectedColumn) < SelectedColumns.Count - 1,
                o => DownColumn(SelectedSelectedColumn)
            );

            AddColumnCommand = new DelegateCommand(
                o => SelectedAvailableColumn != null,
                o => AddColumn(SelectedAvailableColumn)
            );

            RemoveColumnCommand = new DelegateCommand(
                o => SelectedSelectedColumn != null,
                o => RemoveColumn(SelectedSelectedColumn)
            );

            ResetDefaultsCommand = new DelegateCommand(
                o => true,
                o => ResetDefaults()
            );

            SetColumns(columns);
        }

        /// <summary>
        /// Build columns list for available and selected boxes
        /// </summary>
        /// <param name="selectedColumns"></param>
        private void SetColumns(IEnumerable<GuiGridColumn> selectedColumns)
        {
            AvailableColumns.Clear();
            foreach (var col in EventDefinition.Instance.Columns)
            {
                var item = selectedColumns.FirstOrDefault(x => x.ColumnType == col.ColumnType) ??
                    new GuiGridColumn()
                    {
                        ColumnType = col.ColumnType,
                        Width = 100
                    };
                AvailableColumns.Add(item);
            }

            // when adding selected column they will be auto-removed from "available" list
            SelectedColumns.Clear();
            foreach (var item in selectedColumns)
                AddColumn(item);

            SelectedSelectedColumn = SelectedAvailableColumn = null;
        }

        /// <summary>
        /// Add column to "selected" list (and remove from "available" list)
        /// </summary>
        /// <param name="column"></param>
        private void AddColumn(GuiGridColumn column)
        {
            SelectedColumns.Add(column);
            AvailableColumns.Remove(column);
            SelectedSelectedColumn = column;
            SelectedAvailableColumn = null;
        }

        /// <summary>
        /// Remove column from "selected" (and add to "available" list)
        /// </summary>
        /// <param name="column"></param>
        private void RemoveColumn(GuiGridColumn column)
        {
            AvailableColumns.Add(column);
            SelectedColumns.Remove(column);
            SelectedAvailableColumn = column;
            SelectedSelectedColumn = null;
        }

        /// <summary>
        /// Move column up in order
        /// </summary>
        /// <param name="column"></param>
        private void UpColumn(GuiGridColumn column)
        {
            int idx = SelectedColumns.IndexOf(column);
            SelectedColumns.Move(idx, idx - 1);
        }

        /// <summary>
        /// Move column down in order
        /// </summary>
        /// <param name="column"></param>
        private void DownColumn(GuiGridColumn column)
        {
            int idx = SelectedColumns.IndexOf(column);
            SelectedColumns.Move(idx, idx + 1);
        }

        /// <summary>
        /// Save action executed: invoke OnSave event and close the window
        /// </summary>
        private void Save()
        {
            _onSave.Invoke(SelectedColumns);
            CloseAction();
        }

        /// <summary>
        /// Reset all settings to pre-defined defaults
        /// </summary>
        private void ResetDefaults()
        {
            var items = new UserSettings().GetDefaultColumns();
            SetColumns(items);
        }
    }
}
