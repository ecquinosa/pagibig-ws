<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class PackUpDataForm
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.txtRefNum = New System.Windows.Forms.TextBox()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.lblBankID = New System.Windows.Forms.Label()
        Me.rdbRefNum = New System.Windows.Forms.RadioButton()
        Me.rdbApplication = New System.Windows.Forms.RadioButton()
        Me.dtpFrom = New System.Windows.Forms.DateTimePicker()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.dtpTo = New System.Windows.Forms.DateTimePicker()
        Me.progressBar = New System.Windows.Forms.ProgressBar()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'txtRefNum
        '
        Me.txtRefNum.Location = New System.Drawing.Point(110, 52)
        Me.txtRefNum.Name = "txtRefNum"
        Me.txtRefNum.Size = New System.Drawing.Size(193, 20)
        Me.txtRefNum.TabIndex = 0
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(228, 190)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(75, 23)
        Me.Button1.TabIndex = 1
        Me.Button1.Text = "PackUp"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(49, 55)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(49, 13)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "RefNum:"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(12, 9)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(35, 13)
        Me.Label2.TabIndex = 3
        Me.Label2.Text = "Bank:"
        '
        'lblBankID
        '
        Me.lblBankID.AutoSize = True
        Me.lblBankID.Location = New System.Drawing.Point(53, 9)
        Me.lblBankID.Name = "lblBankID"
        Me.lblBankID.Size = New System.Drawing.Size(13, 13)
        Me.lblBankID.TabIndex = 4
        Me.lblBankID.Text = "0"
        '
        'rdbRefNum
        '
        Me.rdbRefNum.AutoSize = True
        Me.rdbRefNum.Location = New System.Drawing.Point(19, 20)
        Me.rdbRefNum.Name = "rdbRefNum"
        Me.rdbRefNum.Size = New System.Drawing.Size(79, 17)
        Me.rdbRefNum.TabIndex = 5
        Me.rdbRefNum.TabStop = True
        Me.rdbRefNum.Text = "By RefNum"
        Me.rdbRefNum.UseVisualStyleBackColor = True
        '
        'rdbApplication
        '
        Me.rdbApplication.AutoSize = True
        Me.rdbApplication.Location = New System.Drawing.Point(19, 98)
        Me.rdbApplication.Name = "rdbApplication"
        Me.rdbApplication.Size = New System.Drawing.Size(149, 17)
        Me.rdbApplication.TabIndex = 6
        Me.rdbApplication.TabStop = True
        Me.rdbApplication.Text = "By Application Day Range"
        Me.rdbApplication.UseVisualStyleBackColor = True
        '
        'dtpFrom
        '
        Me.dtpFrom.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpFrom.Location = New System.Drawing.Point(110, 125)
        Me.dtpFrom.Name = "dtpFrom"
        Me.dtpFrom.Size = New System.Drawing.Size(193, 20)
        Me.dtpFrom.TabIndex = 7
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(71, 129)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(33, 13)
        Me.Label3.TabIndex = 8
        Me.Label3.Text = "From:"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(81, 157)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(23, 13)
        Me.Label4.TabIndex = 10
        Me.Label4.Text = "To:"
        '
        'dtpTo
        '
        Me.dtpTo.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpTo.Location = New System.Drawing.Point(110, 151)
        Me.dtpTo.Name = "dtpTo"
        Me.dtpTo.Size = New System.Drawing.Size(193, 20)
        Me.dtpTo.TabIndex = 9
        '
        'progressBar
        '
        Me.progressBar.Location = New System.Drawing.Point(-1, 267)
        Me.progressBar.Name = "progressBar"
        Me.progressBar.Size = New System.Drawing.Size(351, 10)
        Me.progressBar.TabIndex = 11
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.rdbRefNum)
        Me.Panel1.Controls.Add(Me.txtRefNum)
        Me.Panel1.Controls.Add(Me.Label4)
        Me.Panel1.Controls.Add(Me.Button1)
        Me.Panel1.Controls.Add(Me.dtpTo)
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Controls.Add(Me.Label3)
        Me.Panel1.Controls.Add(Me.rdbApplication)
        Me.Panel1.Controls.Add(Me.dtpFrom)
        Me.Panel1.Location = New System.Drawing.Point(9, 35)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(319, 226)
        Me.Panel1.TabIndex = 12
        '
        'PackUpDataForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(336, 273)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.progressBar)
        Me.Controls.Add(Me.lblBankID)
        Me.Controls.Add(Me.Label2)
        Me.Name = "PackUpDataForm"
        Me.Text = "PackUpDataForm"
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents txtRefNum As Windows.Forms.TextBox
    Friend WithEvents Button1 As Windows.Forms.Button
    Friend WithEvents Label1 As Windows.Forms.Label
    Friend WithEvents Label2 As Windows.Forms.Label
    Friend WithEvents lblBankID As Windows.Forms.Label
    Friend WithEvents rdbRefNum As Windows.Forms.RadioButton
    Friend WithEvents rdbApplication As Windows.Forms.RadioButton
    Friend WithEvents dtpFrom As Windows.Forms.DateTimePicker
    Friend WithEvents Label3 As Windows.Forms.Label
    Friend WithEvents Label4 As Windows.Forms.Label
    Friend WithEvents dtpTo As Windows.Forms.DateTimePicker
    Friend WithEvents progressBar As Windows.Forms.ProgressBar
    Friend WithEvents Panel1 As Windows.Forms.Panel
End Class
