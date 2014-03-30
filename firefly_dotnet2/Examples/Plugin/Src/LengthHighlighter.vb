'==========================================================================
'
'  File:        LengthHighlighter.vb
'  Location:    Firefly.Examples <Visual Basic .Net>
'  Description: 长度检查高亮器
'  Version:     2009.12.01.
'  Author:      F.R.C.
'  Copyright(C) Public Domain
'
'==========================================================================

Option Compare Text

Imports System
Imports System.Drawing
Imports System.Text.RegularExpressions
Imports System.Linq
Imports Firefly
Imports Firefly.Project
Imports TextLocalizer.WQSGPlugin

Public Class Main
    Inherits TextLocalizerBase
    Implements ITextLocalizerTextHighlighter

    Private r As New Regex("\{.*?\}|.|\r|\n", RegexOptions.ExplicitCapture)
    Private Function GetTextStylesForColumn(ByVal TextName As String, ByVal TextIndex As Integer, ByVal TextColumn As Integer, ByVal FormatedText As String) As TextStyle()
        Dim Column = Columns(TextColumn)
        Dim MainColumn = Columns(MainColumnIndex)
        If Column Is MainColumn Then Return Nothing
        If Column.Type = "WQSGIndex" Then Return Nothing

        Dim Length As Integer
        If MainColumn.Type = "WQSGIndex" Then
            Length = DirectCast(MainColumn.Item(TextName), WQSGIndexList).Item(TextIndex).Length
        ElseIf MainColumn.Type = "WQSGText" Then
            Length = DirectCast(MainColumn.Item(TextName), WQSGTextList).Item(TextIndex).Length
        Else
            Return Nothing
        End If

        If Column.Name <> "Translation" AndAlso Column.Name <> "Revision" Then Return Nothing

        Dim Lengths = From m As Match In r.Matches(FormatedText) Select GetByteLength(m.Value)
        Dim Index = 0
        For n = 0 To Lengths.Count - 1
            If Index + Lengths(n) > Length Then
                Return New TextStyle() {New TextStyle With {.Index = n, .Length = FormatedText.Length - n, .ForeColor = Color.Black, .BackColor = Color.Red}}
            End If
            Index += Lengths(n)
        Next
        Return Nothing
    End Function
    Public Function GetTextStyles(ByVal TextName As String, ByVal TextIndex As Integer, ByVal FormatedTexts As System.Collections.Generic.IEnumerable(Of String)) As System.Collections.Generic.IEnumerable(Of Firefly.Project.TextStyle()) Implements Firefly.Project.ITextLocalizerTextHighlighter.GetTextStyles
        Return From i In Enumerable.Range(0, Columns.Count) Select GetTextStylesForColumn(TextName, TextIndex, i, FormatedTexts(i))
    End Function

    Public Function GetByteLength(ByVal Text As String) As Integer
        If Text.Length > 1 Then Return 1
        If Convert.ToInt32(Text(0)) < 128 Then Return 1
        Return 2
    End Function
End Class
