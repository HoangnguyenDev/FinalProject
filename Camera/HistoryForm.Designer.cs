﻿namespace Camera
{
    partial class HistoryForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.metroGrid1 = new MetroFramework.Controls.MetroGrid();
            this.goLeaveBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.admin_dangkythitoeicDataSet = new Camera.admin_dangkythitoeicDataSet();
            this.goLeaveTableAdapter = new Camera.admin_dangkythitoeicDataSetTableAdapters.GoLeaveTableAdapter();
            this.goDTDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.leaveDTDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ownerIDDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.isFinishDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.goAvatarDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewLinkColumn();
            this.LeavePlate = new System.Windows.Forms.DataGridViewLinkColumn();
            this.GoPlate = new System.Windows.Forms.DataGridViewLinkColumn();
            this.OutOCR = new System.Windows.Forms.DataGridViewLinkColumn();
            this.leaveFullDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewLinkColumn();
            this.noteDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.metroGrid1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.goLeaveBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.admin_dangkythitoeicDataSet)).BeginInit();
            this.SuspendLayout();
            // 
            // metroGrid1
            // 
            this.metroGrid1.AllowUserToAddRows = false;
            this.metroGrid1.AllowUserToDeleteRows = false;
            this.metroGrid1.AllowUserToResizeRows = false;
            this.metroGrid1.AutoGenerateColumns = false;
            this.metroGrid1.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.metroGrid1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.metroGrid1.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.metroGrid1.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(174)))), ((int)(((byte)(219)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(198)))), ((int)(((byte)(247)))));
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.metroGrid1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.metroGrid1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.metroGrid1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.goDTDataGridViewTextBoxColumn,
            this.leaveDTDataGridViewTextBoxColumn,
            this.ownerIDDataGridViewTextBoxColumn,
            this.isFinishDataGridViewCheckBoxColumn,
            this.goAvatarDataGridViewTextBoxColumn,
            this.LeavePlate,
            this.GoPlate,
            this.OutOCR,
            this.leaveFullDataGridViewTextBoxColumn,
            this.noteDataGridViewTextBoxColumn});
            this.metroGrid1.DataSource = this.goLeaveBindingSource;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(136)))), ((int)(((byte)(136)))), ((int)(((byte)(136)))));
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(198)))), ((int)(((byte)(247)))));
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.metroGrid1.DefaultCellStyle = dataGridViewCellStyle2;
            this.metroGrid1.EnableHeadersVisualStyles = false;
            this.metroGrid1.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.metroGrid1.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.metroGrid1.Location = new System.Drawing.Point(24, 64);
            this.metroGrid1.Name = "metroGrid1";
            this.metroGrid1.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(174)))), ((int)(((byte)(219)))));
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(198)))), ((int)(((byte)(247)))));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.metroGrid1.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.metroGrid1.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.metroGrid1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.metroGrid1.Size = new System.Drawing.Size(830, 213);
            this.metroGrid1.TabIndex = 0;
            // 
            // goLeaveBindingSource
            // 
            this.goLeaveBindingSource.DataMember = "GoLeave";
            this.goLeaveBindingSource.DataSource = this.admin_dangkythitoeicDataSet;
            // 
            // admin_dangkythitoeicDataSet
            // 
            this.admin_dangkythitoeicDataSet.DataSetName = "admin_dangkythitoeicDataSet";
            this.admin_dangkythitoeicDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // goLeaveTableAdapter
            // 
            this.goLeaveTableAdapter.ClearBeforeFill = true;
            // 
            // goDTDataGridViewTextBoxColumn
            // 
            this.goDTDataGridViewTextBoxColumn.DataPropertyName = "GoDT";
            this.goDTDataGridViewTextBoxColumn.HeaderText = "Thời gian vào";
            this.goDTDataGridViewTextBoxColumn.Name = "goDTDataGridViewTextBoxColumn";
            // 
            // leaveDTDataGridViewTextBoxColumn
            // 
            this.leaveDTDataGridViewTextBoxColumn.DataPropertyName = "LeaveDT";
            this.leaveDTDataGridViewTextBoxColumn.HeaderText = "Thời gian về";
            this.leaveDTDataGridViewTextBoxColumn.Name = "leaveDTDataGridViewTextBoxColumn";
            this.leaveDTDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            // 
            // ownerIDDataGridViewTextBoxColumn
            // 
            this.ownerIDDataGridViewTextBoxColumn.DataPropertyName = "OwnerID";
            this.ownerIDDataGridViewTextBoxColumn.HeaderText = "Mã";
            this.ownerIDDataGridViewTextBoxColumn.Name = "ownerIDDataGridViewTextBoxColumn";
            // 
            // isFinishDataGridViewCheckBoxColumn
            // 
            this.isFinishDataGridViewCheckBoxColumn.DataPropertyName = "IsFinish";
            this.isFinishDataGridViewCheckBoxColumn.HeaderText = "Đã hoàn thành";
            this.isFinishDataGridViewCheckBoxColumn.Name = "isFinishDataGridViewCheckBoxColumn";
            // 
            // goAvatarDataGridViewTextBoxColumn
            // 
            this.goAvatarDataGridViewTextBoxColumn.DataPropertyName = "GoAvatar";
            this.goAvatarDataGridViewTextBoxColumn.HeaderText = "Khuôn mặt đi";
            this.goAvatarDataGridViewTextBoxColumn.Name = "goAvatarDataGridViewTextBoxColumn";
            this.goAvatarDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.goAvatarDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // LeavePlate
            // 
            this.LeavePlate.DataPropertyName = "LeavePlate";
            this.LeavePlate.HeaderText = "Khuôn mặt về";
            this.LeavePlate.Name = "LeavePlate";
            this.LeavePlate.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // GoPlate
            // 
            this.GoPlate.DataPropertyName = "GoPlate";
            this.GoPlate.HeaderText = "Biển số xe đi";
            this.GoPlate.Name = "GoPlate";
            this.GoPlate.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // OutOCR
            // 
            this.OutOCR.DataPropertyName = "OutOCR";
            this.OutOCR.HeaderText = "Biển số xe về";
            this.OutOCR.Name = "OutOCR";
            this.OutOCR.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.OutOCR.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // leaveFullDataGridViewTextBoxColumn
            // 
            this.leaveFullDataGridViewTextBoxColumn.DataPropertyName = "LeaveFull";
            this.leaveFullDataGridViewTextBoxColumn.HeaderText = "Hình ảnh đi đầy đủ";
            this.leaveFullDataGridViewTextBoxColumn.Name = "leaveFullDataGridViewTextBoxColumn";
            this.leaveFullDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.leaveFullDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // noteDataGridViewTextBoxColumn
            // 
            this.noteDataGridViewTextBoxColumn.DataPropertyName = "Note";
            this.noteDataGridViewTextBoxColumn.HeaderText = "Note";
            this.noteDataGridViewTextBoxColumn.Name = "noteDataGridViewTextBoxColumn";
            // 
            // HistoryForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(871, 300);
            this.Controls.Add(this.metroGrid1);
            this.MaximizeBox = false;
            this.Name = "HistoryForm";
            this.Resizable = false;
            this.Text = "Xem lịch sử";
            this.Load += new System.EventHandler(this.HistoryForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.metroGrid1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.goLeaveBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.admin_dangkythitoeicDataSet)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private MetroFramework.Controls.MetroGrid metroGrid1;
        private admin_dangkythitoeicDataSet admin_dangkythitoeicDataSet;
        private System.Windows.Forms.BindingSource goLeaveBindingSource;
        private admin_dangkythitoeicDataSetTableAdapters.GoLeaveTableAdapter goLeaveTableAdapter;
        private System.Windows.Forms.DataGridViewTextBoxColumn goOcgDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn leaveOcgDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn oCRDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn goDTDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn leaveDTDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn ownerIDDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isFinishDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewLinkColumn goAvatarDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewLinkColumn LeavePlate;
        private System.Windows.Forms.DataGridViewLinkColumn GoPlate;
        private System.Windows.Forms.DataGridViewLinkColumn OutOCR;
        private System.Windows.Forms.DataGridViewLinkColumn leaveFullDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn noteDataGridViewTextBoxColumn;
    }
}