   M S B A M L   `   `   `  ����   testTensileMachineGraphics9    3testTensileMachineGraphics.Options.OptionsAnimation   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     -     � Title$-  'Podešavanja opcija u animacijskom modu��$	��350��$
��1200��.���  � ColumnDefinitions �� $��10 �$��10���� $
��1.8* ��� $��10 �$��10���� $��* ��� $��10 �$��10��.+�� -   $��gridGraphicOptions��$��0��$��1�� � RowDefinitions �� $��40 ��� $��26 �$��26���� $��26 �$��26���� $��26 �$��26���� $��26 �$��26���� $��26 �$��26���� $��40 ��� $��26 �$��26���� $��26 �$��26���� $��26 �$��26���� $��26 �$��26��.+��� -   $��lblOptionsGraphic��$
��Left=�$��Center=�$��20(�$*��$Opcije grafika animacijskog moda :  ��� -   $��gridGraphicDisplay��$��1��$��1�� �� $��10 �$��10���� $��* ��� $��10 �$��10���� $��* ��� $��10 �$��10���� $��* ��� $��10 �$��10��.+��� -   $��
lblDisplay��$��1��$��0��$0��*Izaberite prikaz grafika u online modu :  ��	� -   $��rbtnContinuous�� 	� 	GroupName$ graphicDisplay��$��15��$��15��$��3��$��Stretch=�$��Center=�$ ��kontinualan prikaz grafika��	� -   $��rbtnDiscrete��$ graphicDisplay��$��15��$��15��$��5��$��Stretch=�$��Center=�$��diskretan prikaz grafika��� -   $��gridGraphicRefresh��$��1��$��2�� �� $��10 �$��10���� $	��350 �$	��350���� $��10 �$��10���� $��0.12* ��� $��10 �$��10���� $
��0.5* ��� $��10 �$��10��.+��� -	   $��
lblRefresh��$��1��$��0��$I��CIzaberite vremenski interval osvežavanja grafika u online modu :  ���� -
   $��
cmbRefresh��$��3��$��0��
�������$��Stretch=�$��25��$
��Auto��.@��� .��100�� .��60�� .��25�� -   $��lblRefreshEnd��$��5��$��0��$
��Left=�$��Center=�$�� [Hz] ��� -   $��gridGraphicResolutionCon��$��1��$��3�� �� $��10 �$��10���� $	��350 �$	��350���� $��10 �$��10���� $��0.12* ��� $��10 �$��10���� $
��0.5* ��� $��10 �$��10��.+��� -   $��lblResolutionCon��$��1��$��0��$3��-Izaberite rezoluciju kontinualnog grafika :  ���� -   $��tfResolutionCon��$��3��$��0��
�������$��Stretch=�$��25��$
��Auto���� -   $��lblResolutionConEnd��$��5��$��0��$
��Left=�$��Center=�$�� [ceo broj]��� -   $��gridGraphicResolutionDis��$��1��$��4�� �� $��10 �$��10���� $	��350 �$	��350���� $��10 �$��10���� $��0.12* ��� $��10 �$��10���� $
��0.5* ��� $��10 �$��10��.+��� -   $��lblResolutionDis��$��1��$��0��$1��+Izaberite rezoluciju diskretnog grafika :  ���� -   $��tfResolutionDis��$��3��$��0��
�������$��Stretch=�$��25��$
��Auto���� -   $��lblResolutionDisEnd��$��5��$��0��$
��Left=�$��Center=�$�� [ceo broj]���� -   $��lblLoadFilepath��$
��Left=�$��Center=�$��6��$��20(�$#��Učitavanje putanje fajla :  ��� -   $��gridPlottingFilePath��$��1��$��7�� �� $��10 �$��10���� $	��335 �$	��335���� $��10 �$��10���� $��30 �$��30���� $��10 �$��10���� $
��0.5* ��� $��10 �$��10��.+��� -   $��lblFilepath��$��1��$��0��$K��EIzaberite putanju fajla koji želite da prikažete u offline modu :  ���� -   $��btnChooseDatabasePath��$	��...��$��20��$��20��$��Center=�$��Center=�$��3��� -   $!��gridPlottingFilePathTextBox��$��1��$��8�� �� $��10 �$��10���� $	��700 �$	��700���� $��* �.+��� -   $��tfFilepathPlotting��$��1��$��0��
�������$��Stretch=�$��25��$
��Auto��� -   $��gridGraphicSaveButton��$��1��$��9�� �� $��10 �$��10���� $	��185 �$	��185���� $��* �.+��� -   $��btnSaveAnimationOptions��$��1��$��Stretch=�$��Center=�$��Zapamti promene opcija��� -   $��gridGraphicCalibration��$��0��$��3�� �� $��40 ��� $��26 �$��26���� $��26 �$��26���� $��26 �$��26���� $��40 ��� $��26 �$��26���� $��26 �$��26���� $��26 �$��26���� $��26 �$��26���� $��26 �$��26���� $��* �.+��� -   $��lblGraphicCalibration��$
��Left=�$��Center=�$��20(�$��Kalibracija  ���� -   $ ��lblGraphicCalibrationForce��$��1��$
��Left=�$��Center=�$��   Kalibracija sile : ��� -   $��gridGraphicCalForce��$��1��$��2�� �� $��10 �$��10���� $	��205 �$	��205���� $��10 �$��10���� $
��0.5* ��� $��10 �$��10���� $
��0.5* ��� $��10 �$��10��.+��� -    $��lblCalForceDivide��$��1��$��0��$+��%Koeficijent sa kojim se deli sila :  ���� -!   $��tfCalForceDivide��$��3��$��0��
�������$��Stretch=�$��25��$
��Auto���� -"   $��lblCalForceDivideEnd��$��5��$��0��$
��Left=�$��Center=�$�� [realan broj] ��� -#   $��gridGraphicCalForce2��$��1��$��3�� �� $��10 �$��10���� $	��205 �$	��205���� $��10 �$��10���� $
��0.5* ��� $��10 �$��10���� $
��0.5* ��� $��10 �$��10��.+��� -$   $��lblCalForceMultiple��$��1��$��0��$-��'Koeficijent sa kojim se množi sila :  ���� -%   $��tfCalForceMultiple��$��3��$��0��
�������$��Stretch=�$��25��$
��Auto���� -&   $��lblCalForceMultipleEnd��$��5��$��0��$
��Left=�$��Center=�$�� [realan broj] ���� -'   $%��lblGraphicCalibrationElongation��$��5��$
��Left=�$��Center=�$"��   Kalibracija izduženja : ��� -(   $��gridGraphicCalElon��$��1��$��6�� �� $��10 �$��10���� $	��205 �$	��205���� $��10 �$��10���� $
��0.5* ��� $��10 �$��10���� $
��0.5* ��� $��10 �$��10��.+��� -)   $��lblCalElonDivide��$��1��$��0��$��11(�$/��)Koeficijent sa kojim se deli izduženje :���� -*   $��tfCalElonDivide��$��3��$��0��
�������$��Stretch=�$��25��$
��Auto���� -+   $��lblCalElonDivideEnd��$��5��$��0��$
��Left=�$��Center=�$�� [realan broj] ��� -,   $��gridGraphicCalElon2��$��1��$��7�� �� $��10 �$��10���� $	��205 �$	��205���� $��10 �$��10���� $
��0.5* ��� $��10 �$��10���� $
��0.5* ��� $��10 �$��10��.+��� --   $��lblCalElonMultiple��$��1��$��0��$��11(�$3��-Koeficijent sa kojim se množi izduženje :  ���� -.   $��tfCalElonMultiple��$��3��$��0��
�������$��Stretch=�$��25��$
��Auto���� -/   $��lblCalElonMultipleEnd��$��5��$��0��$
��Left=�$��Center=�$�� [realan broj] ��