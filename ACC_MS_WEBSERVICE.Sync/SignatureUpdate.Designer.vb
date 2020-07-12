<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class SignatureUpdate
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
        Me.Button1 = New System.Windows.Forms.Button()
        Me.picSignature = New System.Windows.Forms.PictureBox()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.txtRefNum = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Button3 = New System.Windows.Forms.Button()
        Me.picSigLoad = New System.Windows.Forms.PictureBox()
        CType(Me.picSignature, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.picSigLoad, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(532, 237)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(75, 23)
        Me.Button1.TabIndex = 0
        Me.Button1.Text = "Browse"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'picSignature
        '
        Me.picSignature.Location = New System.Drawing.Point(13, 13)
        Me.picSignature.Name = "picSignature"
        Me.picSignature.Size = New System.Drawing.Size(596, 218)
        Me.picSignature.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.picSignature.TabIndex = 1
        Me.picSignature.TabStop = False
        '
        'Button2
        '
        Me.Button2.Location = New System.Drawing.Point(532, 492)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(75, 23)
        Me.Button2.TabIndex = 2
        Me.Button2.Text = "Replace"
        Me.Button2.UseVisualStyleBackColor = True
        '
        'txtRefNum
        '
        Me.txtRefNum.Location = New System.Drawing.Point(232, 494)
        Me.txtRefNum.Name = "txtRefNum"
        Me.txtRefNum.Size = New System.Drawing.Size(215, 20)
        Me.txtRefNum.TabIndex = 3
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(177, 497)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(49, 13)
        Me.Label1.TabIndex = 4
        Me.Label1.Text = "RefNum:"
        '
        'Button3
        '
        Me.Button3.Location = New System.Drawing.Point(452, 492)
        Me.Button3.Name = "Button3"
        Me.Button3.Size = New System.Drawing.Size(75, 23)
        Me.Button3.TabIndex = 5
        Me.Button3.Text = "Load"
        Me.Button3.UseVisualStyleBackColor = True
        '
        'picSigLoad
        '
        Me.picSigLoad.Location = New System.Drawing.Point(11, 266)
        Me.picSigLoad.Name = "picSigLoad"
        Me.picSigLoad.Size = New System.Drawing.Size(596, 218)
        Me.picSigLoad.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.picSigLoad.TabIndex = 6
        Me.picSigLoad.TabStop = False
        '
        'SignatureUpdate
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(619, 527)
        Me.Controls.Add(Me.picSigLoad)
        Me.Controls.Add(Me.Button3)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.txtRefNum)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.picSignature)
        Me.Controls.Add(Me.Button1)
        Me.Name = "SignatureUpdate"
        Me.Text = "SignatureUpdate"
        CType(Me.picSignature, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.picSigLoad, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents Button1 As Windows.Forms.Button
    Friend WithEvents picSignature As Windows.Forms.PictureBox
    Friend WithEvents Button2 As Windows.Forms.Button
    Friend WithEvents txtRefNum As Windows.Forms.TextBox
    Friend WithEvents Label1 As Windows.Forms.Label
    Friend WithEvents Button3 As Windows.Forms.Button
    Friend WithEvents picSigLoad As Windows.Forms.PictureBox
End Class
