   M S B A M L   `   `   `  ����   testTensileMachineGraphics=    7testTensileMachineGraphics.Options.OptionsForYungsModuo   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     -     � Title$  OptionsForYungsModuo��$	��350��$	��420��.��� 
������� � 	Resources  �� 
TargetType �� Property �� Value��%�  '��       '�� F     '	� �     '�� �     '��     �� " ��.���  � ��$ 15(���  � ��$ Times New Roman)��� " ��.���  � ��$ 15(���  � ��$ Times New Roman)��� " 	�.���  � ��$
 14.5(���  � ��$ Times New Roman)��� " ��.���  � ��$
 14.5(���  � ��$ Times New Roman)��� " ��.���  � ��
 �������  � ��
 �   � � ColumnDefinitions �� $��10 �$��10���� $��* ��� $��10 �$��10�� � RowDefinitions �� $��10 �$��10���� $��35 ��� $��10 ��� $��45 ��� $��30 ��� $��30 ��� $��26 ��� $��28 ��� $��28 ��� $��28 ��� $��32 ��� $��28 ��� $��28 ��� $��28 �.+��� -   $��lblYungCaption1��$��1��$��1��$��20(�$
��Bold"�$��Center=�$��Center=�$ ��   Opcije za racunanje    ���� -   $��lblYungCaption2��$��1��$��3��$��20(�$
��Bold"�$��Center=�$��Center=�$"��Jungovog modula elasticnosti���� -   $��lblYungLegend��$��1��$��4��$
��Bold"�$
��Left=�$��Center=�$��
Legenda : ���� -   $��	lblxelas1��$��1��$��5��$
��Bold"�$
��Left=�$��Center=�$7��1Xelas - očekivana granica elastičnosti po x-osi���� -   $��	lblxelas2��$��1��$��6��$y�Visible=�$
��Bold"�$
��Left=�$��Center=�$8��2Yelas - vrednost napona u MPa kojem odgovara Xelas��� -   $��gridforXelas��$��1��$��7�� �� $
��1.2* ��� $��10 �$��10���� $
��1.2* ��� $��10 �$��10���� $��* ��� $��10 �$��10��.+��� -   $��lblxelas��$��0��$��0��$
��Bold"�$��Right=�$��Center=�$��
Xelas  :  ���� -	   $��tfXelas��$��2��$��Stretch=�$��25��$
��Auto���� -
   $��lblEndtfXelas��$��4��$��0��$
��Left=�$��Center=�$	��[%]��� -   $��gridforprocspustanja��$��1��$��8��$y�Visible=� �� $
��1.2* ��� $��10 �$��10���� $
��1.2* ��� $��10 �$��10���� $��* ��� $��10 �$��10��.+��� -   $��lblprocspustanja��$��0��$��0��$
��Bold"�$��Right=�$��Center=�$��Pr. spustanja  :  ���� -   $��tfprocspustanja��$��2��$��Stretch=�$��25��$
��Auto���� -   $��lblprocspustanjaend��$��4��$��0��$
��Left=�$��Center=�$	��[%]���� -   $��lblyung1��$��1��$��10��$
��Bold"�$y�Visible=�$
��Left=�$��Center=�$?��9granica elasticnosti [Re] = (100-Pr. spustanja)/100*Yelas��