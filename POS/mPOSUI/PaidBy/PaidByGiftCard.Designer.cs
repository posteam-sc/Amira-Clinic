﻿namespace POS
{
    partial class PaidByGiftCard
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PaidByGiftCard));
            this.lblTotalCost = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtGiftCardId = new System.Windows.Forms.TextBox();
            this.btnSubmit = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblAmountFromGiftCard = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtCash = new System.Windows.Forms.TextBox();
            this.lblPayableAmount = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.lblChangesText = new System.Windows.Forms.Label();
            this.cboBankPayment = new System.Windows.Forms.ComboBox();
            this.cboPaymentMethod = new System.Windows.Forms.ComboBox();
            this.lblBank = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblTotalCost
            // 
            this.lblTotalCost.AutoSize = true;
            this.lblTotalCost.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalCost.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.lblTotalCost.Location = new System.Drawing.Point(161, 193);
            this.lblTotalCost.Name = "lblTotalCost";
            this.lblTotalCost.Size = new System.Drawing.Size(35, 15);
            this.lblTotalCost.TabIndex = 9;
            this.lblTotalCost.Text = "0000";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label2.Location = new System.Drawing.Point(18, 195);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(61, 15);
            this.label2.TabIndex = 8;
            this.label2.Text = "Total Cost";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label1.Location = new System.Drawing.Point(18, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "&Gift Card Id";
            // 
            // txtGiftCardId
            // 
            this.txtGiftCardId.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtGiftCardId.Location = new System.Drawing.Point(161, 18);
            this.txtGiftCardId.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtGiftCardId.Name = "txtGiftCardId";
            this.txtGiftCardId.Size = new System.Drawing.Size(198, 21);
            this.txtGiftCardId.TabIndex = 1;
            this.txtGiftCardId.Leave += new System.EventHandler(this.txtGiftCardId_Leave);
            // 
            // btnSubmit
            // 
            this.btnSubmit.BackColor = System.Drawing.Color.Transparent;
            this.btnSubmit.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(223)))), ((int)(((byte)(223)))));
            this.btnSubmit.FlatAppearance.BorderSize = 0;
            this.btnSubmit.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(223)))), ((int)(((byte)(223)))));
            this.btnSubmit.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(223)))), ((int)(((byte)(223)))));
            this.btnSubmit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSubmit.Image = global::POS.Properties.Resources.save_big1;
            this.btnSubmit.Location = new System.Drawing.Point(147, 288);
            this.btnSubmit.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnSubmit.Name = "btnSubmit";
            this.btnSubmit.Size = new System.Drawing.Size(104, 38);
            this.btnSubmit.TabIndex = 14;
            this.btnSubmit.UseVisualStyleBackColor = false;
            this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.Transparent;
            this.btnCancel.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(223)))), ((int)(((byte)(223)))));
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(223)))), ((int)(((byte)(223)))));
            this.btnCancel.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(223)))), ((int)(((byte)(223)))));
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Image = global::POS.Properties.Resources.cancel_big3;
            this.btnCancel.Location = new System.Drawing.Point(251, 288);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(104, 38);
            this.btnCancel.TabIndex = 15;
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lblAmountFromGiftCard
            // 
            this.lblAmountFromGiftCard.AutoSize = true;
            this.lblAmountFromGiftCard.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAmountFromGiftCard.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.lblAmountFromGiftCard.Location = new System.Drawing.Point(161, 57);
            this.lblAmountFromGiftCard.Name = "lblAmountFromGiftCard";
            this.lblAmountFromGiftCard.Size = new System.Drawing.Size(14, 15);
            this.lblAmountFromGiftCard.TabIndex = 5;
            this.lblAmountFromGiftCard.Text = "0";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label6.Location = new System.Drawing.Point(18, 59);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(130, 15);
            this.label6.TabIndex = 4;
            this.label6.Text = "Amount from GiftCards";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label4.Location = new System.Drawing.Point(18, 163);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(96, 15);
            this.label4.TabIndex = 6;
            this.label4.Text = "Recieve Amount";
            // 
            // txtCash
            // 
            this.txtCash.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCash.Location = new System.Drawing.Point(161, 160);
            this.txtCash.Name = "txtCash";
            this.txtCash.Size = new System.Drawing.Size(194, 21);
            this.txtCash.TabIndex = 7;
            this.txtCash.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtCash_KeyPress);
            this.txtCash.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtCash_KeyUp);
            // 
            // lblPayableAmount
            // 
            this.lblPayableAmount.AutoSize = true;
            this.lblPayableAmount.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPayableAmount.ForeColor = System.Drawing.SystemColors.WindowText;
            this.lblPayableAmount.Location = new System.Drawing.Point(161, 224);
            this.lblPayableAmount.Name = "lblPayableAmount";
            this.lblPayableAmount.Size = new System.Drawing.Size(14, 15);
            this.lblPayableAmount.TabIndex = 11;
            this.lblPayableAmount.Text = "0";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label7.Location = new System.Drawing.Point(18, 226);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(96, 15);
            this.label7.TabIndex = 10;
            this.label7.Text = "Payable Amount";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label5.Location = new System.Drawing.Point(18, 258);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(56, 15);
            this.label5.TabIndex = 12;
            this.label5.Text = "Changes";
            // 
            // lblChangesText
            // 
            this.lblChangesText.AutoSize = true;
            this.lblChangesText.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblChangesText.ForeColor = System.Drawing.SystemColors.WindowText;
            this.lblChangesText.Location = new System.Drawing.Point(161, 256);
            this.lblChangesText.Name = "lblChangesText";
            this.lblChangesText.Size = new System.Drawing.Size(14, 15);
            this.lblChangesText.TabIndex = 13;
            this.lblChangesText.Text = "0";
            // 
            // cboBankPayment
            // 
            this.cboBankPayment.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboBankPayment.FormattingEnabled = true;
            this.cboBankPayment.Location = new System.Drawing.Point(161, 122);
            this.cboBankPayment.Name = "cboBankPayment";
            this.cboBankPayment.Size = new System.Drawing.Size(121, 21);
            this.cboBankPayment.TabIndex = 19;
            this.cboBankPayment.Visible = false;
            // 
            // cboPaymentMethod
            // 
            this.cboPaymentMethod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboPaymentMethod.FormattingEnabled = true;
            this.cboPaymentMethod.Location = new System.Drawing.Point(161, 91);
            this.cboPaymentMethod.Name = "cboPaymentMethod";
            this.cboPaymentMethod.Size = new System.Drawing.Size(121, 21);
            this.cboPaymentMethod.TabIndex = 18;
            this.cboPaymentMethod.SelectedIndexChanged += new System.EventHandler(this.cboPaymentMethod_SelectedIndexChanged);
            // 
            // lblBank
            // 
            this.lblBank.AutoSize = true;
            this.lblBank.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBank.Location = new System.Drawing.Point(18, 125);
            this.lblBank.Name = "lblBank";
            this.lblBank.Size = new System.Drawing.Size(86, 15);
            this.lblBank.TabIndex = 16;
            this.lblBank.Text = "Bank Payment";
            this.lblBank.Visible = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(18, 94);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(100, 15);
            this.label3.TabIndex = 17;
            this.label3.Text = "Payment Method";
            // 
            // PaidByGiftCard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(389, 359);
            this.Controls.Add(this.cboBankPayment);
            this.Controls.Add(this.cboPaymentMethod);
            this.Controls.Add(this.lblBank);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.lblChangesText);
            this.Controls.Add(this.lblPayableAmount);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.lblTotalCost);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtCash);
            this.Controls.Add(this.lblAmountFromGiftCard);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtGiftCardId);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnSubmit);
            this.Controls.Add(this.btnCancel);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "PaidByGiftCard";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PaidByGiftCard";
            this.Load += new System.EventHandler(this.PaidByGiftCard_Load);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.PaidByGiftCard_MouseMove);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSubmit;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtGiftCardId;
        private System.Windows.Forms.Label lblTotalCost;
        private System.Windows.Forms.Label lblAmountFromGiftCard;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtCash;
        private System.Windows.Forms.Label lblPayableAmount;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lblChangesText;
        private System.Windows.Forms.ComboBox cboBankPayment;
        private System.Windows.Forms.ComboBox cboPaymentMethod;
        private System.Windows.Forms.Label lblBank;
        private System.Windows.Forms.Label label3;
    }
}