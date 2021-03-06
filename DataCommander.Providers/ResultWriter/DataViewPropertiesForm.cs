﻿using Foundation.Diagnostics.Contracts;

namespace DataCommander.Providers.ResultWriter
{
    using System;
    using System.Windows.Forms;

    internal partial class DataViewPropertiesForm : Form
    {
        private readonly DataViewProperties _properties = new DataViewProperties();

        public DataViewPropertiesForm(DataViewProperties properties)
        {
            FoundationContract.Requires<ArgumentException>(properties != null);

            _properties = properties;
            InitializeComponent();

            rowFilterTextBox.Text = properties.RowFilter;
            sortTextBox.Text = properties.Sort;
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            _properties.RowFilter = rowFilterTextBox.Text;
            _properties.Sort = sortTextBox.Text;
            DialogResult = DialogResult.OK;
        }
    }
}