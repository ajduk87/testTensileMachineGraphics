   M S B A M L   `   `   `  ����   testTensileMachineGraphics8    2testTensileMachineGraphics.Options.OptionsPlotting   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     -     � 
ResizeMode$  NoResize=� � Title$- 'Podešavanja opcija iscrtavanja grafika��$	��950��$	��645��.��� .+��� ��.  $
��Auto=�.��� -   $��gridofflineoptions��$��3��
������� � 	Resources  �� 
TargetType �� Property �� Value   buttonTemplate A�Name �� 
StartPoint �� EndPoint	 � Offset
 � Color �� RelativeSource �� Path��%d  '��       '�� F     '�� �     &	  �     �� " ��.���  � ��$ 14(���  � ��$ Times New Roman)��� " ��.���  � ��$
 14.5(���  � ��$ Times New Roman)��� " ��.���  � ��
 �������  � ��
 �   ��� .R�� .+�A� $ outerCircle��$��10��$��10������ $	 0,0&�$	 0,1&�.��� $	 1Q�$
 Black��� $	 1Q�$
 Black��A� $��8��$��8������ $	 0,0&�$	 0,1&�.��� $	 1Q�
 ��  �� *TemplatedParent+$ Background.Color�� $	 1Q�
 ��  �� *TemplatedParent+$ Background.Color�(� .���� $��20q�#��z�� � ColumnDefinitions �� $��10 �$��10���� $��* ��� $��10 �$��10�� � RowDefinitions �� $��10 �$��10���� $��35 ��� $��10 ��� $��10 ��� $��26 ��� $��26 ��� $��10 ��� $��10 ��� $��26 ��� $��26 ��� $��26 ��� $��45 ��� $��26 ��� $��26 ��� $��26 ��� $��26 ��� $��26 ��� $��26 ��� $��45 ��� $��26 ��� $��26 ��� $��26 ��� $��26 ��� $��26 ��� $��45 ��� $��26 ��� $��4 ��� $��35 ��� $��4 ��� $��35 ��� $��4 ��� $��35 ��� $��10 ��� $��10 ��� $��45 ��� $��26 ��� $��35 ��� $��26 ��� $��26 ��� $��26 ��� $��26 �.+��� -   $��grboxEpruvetaKidanje��$��Kidanje��$��18(�
���   �
���   �$��33��$��0��$��4��$��3���� -   $!��grboxEpruvetaPrelazniPeriod��$��Prelazni period��$��18(�
���   �
���   �$��23��$��0��$��10��$��3���� -   $&�� grboxEpruvetaAutomatskoFitovanje��$��Automatsko fitovanje��$��18(�
���   �
���   �$��17��$��0��$��6��$��3���� -   $��grboxEpruvetaKalibracija��$��Kalibracija��$��18(�
���   �
���   �$��6��$��0��$��11��$��3���� -   $��grboxEpruvetaRezolucija��$��
Rezolucija��$��18(�
���   �
���   �$��2��$��0��$��4��$��3���� -   $,��&grboxEpruvetaNepodrazumevaniModkidanja��$!��Nepodrazumevani mod kidanja��$
��Left=�$	��270��$��18(�
���   �
���   �$��37��$��0��$��4��$��2���� -	   $#��grboxEpruvetaOpsegRacunanjaNa��$��Opseg računanja n-a��$
��Left=�$��	275,0,0,0q�$	��336��$��18(�
���   �
���   �$��37��$��1��$��4��$��2���� -
   $��lblOptionsCaption��$��Opcije offline moda��$��1��$��1��$
��Left=�$��Center=�$��20(�� -   $��gridResolution��$��1��$��4�� �� $��3* ��� $��10 �$��10���� $
��1.5* ��� $��10 �$��10���� $��2* ��� $��10 �$��10��.+��� -   $��lblresolution��$��0��$��0��$��Right=�$��Center=�$$��Rezolucija prikaza grafika :  ���� -   $��tfResolution��$��2��$��0��$��Stretch=�$��20��$
��Auto���� -   $��lblresolutionEnd��$��4��$��0��$
��Left=�$��Center=�$��
[ceo broj]��� -   $��gridDerivationResolution��$��1��$��5�� �� $��3* ��� $��10 �$��10���� $
��1.5* ��� $��10 �$��10���� $��2* ��� $��10 �$��10��.+��� -   $��lblDerivationResolution��$��0��$��0��$��Right=�$��Center=�$y�Hidden=�$&�� Rezolucija računanja izvoda :  ���� -   $��tfDerivationResolution��$��2��$��0��$��Stretch=�$y�Hidden=�$��20��$
��Auto���� -   $ ��lblDerivationResolutionEnd��$��4��$��0��$
��Left=�$��Center=�$y�Hidden=�$��[realan broj]���� -   $ ��lblGraphicCalibrationForce��$��1��$��8��$��20(�$
��Bold"�$��Center=�$��Center=�$��   Kalibracija sile   ��� -   $��gridNutnDivide��$��1��$��9�� �� $��3* ��� $��10 �$��10���� $
��1.5* ��� $��10 �$��10���� $��2* ��� $��10 �$��10��.+��� -   $��lblCalForceDivide��$��0��$��0��$��Right=�$��Center=�$��Delilac sile :  ���� -   $��tfCalForceDivide��$��2��$��0�� �� 
IsReadOnly . $��Stretch=�$��22��$
��Auto���� -   $��lblCalForceDivideEnd��$��4��$��0��$
��Left=�$��Center=�$��[realan broj]��� -   $��gridNutnMultiple��$��1��$��10�� �� $��3* ��� $��10 �$��10���� $
��1.5* ��� $��10 �$��10���� $��2* ��� $��10 �$��10��.+��� -   $��lblCalForceMultiple��$��0��$��0��$��Right=�$��Center=�$��Množitelj sile :  ���� -   $��tfCalForceMultiple��$��2��$��0�� . $��Stretch=�$��22��$
��Auto���� -   $��lblCalForceMultipleEnd��$��4��$��0��$
��Left=�$��Center=�$��[realan broj]���� -   $%��lblGraphicCalibrationElongation��$��11��$��1��$��20(�$
��Bold"�$��Center=�$��Center=�$,��&   Kalibracija izduženja[traverza]   ��� -   $��gridElonDivide��$��1��$��12�� �� $��3* ��� $��10 �$��10���� $
��1.5* ��� $��10 �$��10���� $��2* ��� $��10 �$��10��.+��� -   $��lblCalElonDivide��$��0��$��0��$��Right=�$��Center=�$��Delilac izduženja :  ���� -   $��tfCalElonDivide��$��2��$��22�� . $
��Auto��$��0,0,0,1q��� -    $��lblCalElonDivideEnd��$��4��$��0��$
��Left=�$��Center=�$��[realan broj]��� -!   $��gridElonMultiple��$��1��$��13�� �� $��3* ��� $��10 �$��10���� $
��1.5* ��� $��10 �$��10���� $��2* ��� $��10 �$��10��.+��� $��0��$��0��$��Right=�$��Center=�$��Množitelj izduženja :  ���� -"   $��tfCalElonMultiple��$��2��$��0�� . $��Stretch=�$��22��$
��Auto���� -#   $��lblCalElonMultipleEnd��$��4��$��0��$
��Left=�$��Center=�$��[realan broj]���� -$   $&�� lblGraphicCalibrationElongation2��$��14��$��1��$��20(�$
��Bold"�$��Center=�$��Center=�$2��,   Kalibracija izduženja[ekstenziometar]   ��� -%   $��gridElonDivide2��$��1��$��15�� �� $��3* ��� $��10 �$��10���� $
��1.5* ��� $��10 �$��10���� $��2* ��� $��10 �$��10��.+��� -&   $��lblCalElonDivide2��$��0��$��0��$��Right=�$��Center=�$��Delilac izduženja :  ���� -'   $��tfCalElonDivide2��$��2��$��22��$
��Auto��$��0,0,0,1q��� -(   $��lblCalElonDivideEnd2��$��4��$��0��$
��Left=�$��Center=�$��[realan broj]��� -)   $��gridElonMultiple2��$��1��$��16�� �� $��3* ��� $��10 �$��10���� $
��1.5* ��� $��10 �$��10���� $��2* ��� $��10 �$��10��.+��� $��0��$��0��$��Right=�$��Center=�$��Množitelj izduženja :  ���� -*   $��tfCalElonMultiple2��$��2��$��0��$��Stretch=�$��22��$
��Auto���� -+   $��lblCalElonMultipleEnd2��$��4��$��0��$
��Left=�$��Center=�$��[realan broj]���� -,   $��lblFittingAutomatic��$��18��$��1��$��20(�$
��Bold"�$��Center=�$��Center=�$��   X osa   ��$��25��$��298,2,181,18q�$	��108���� --   $��lblFittingAutomatic2��$��18��$��1��$��20(�$
��Bold"�$��Center=�$��Center=�$��Y osa   ��$��25��$��205,2,308,18q�$��88��� -.   $��
gridpoint1��$��19��$��1�� �� $	��175 ��� $��30 �$��30���� $
��Auto ��� $��10 �$��10���� $	��175 �.+��� -/   $��stylePoint1��$��1��$��10��$��10��#��[  
����  �� -0   $��gridAutoPoints��$��2�� �� $��20 �$��20���� $
��Auto ��� $��30 �$��30���� $��* ��� $��35 �$��35��.+��� -1   $��lblordnum1auto��$��0��$��Center=�$��Center=�$��T1���� -2   $��tffittingAutoProcent1��$��1��$
��Left=�$��Center=��� -3   $��	lblprauto��$��2��$��2��$��Center=�$��Center=�$	��[%]���� -4   $��lblmaxByXAxis��$��4��$
��Left=�$��Center=�$#��Max dozvoljena vr. po x-u :  ��� -5   $��
gridpoint2��$��20��$��1�� �� $	��175 ��� $��30 �$��30���� $
��Auto ��� $��10 �$��10���� $	��175 ��� $	��99* �.+��� -6   $��stylePoint2��$��1��$��10��$��10��#��[  
��� ���� -7   $��gridAutoPoints2��$��2�� �� $��20 �$��20���� $��* ��� $��30 �$��30���� $��* ��� $��35 �$��35��.+��� -8   $��lblordnum2auto��$��0��$��Center=�$��Center=�$��T2���� -9   $��tffittingAutoProcent2��$��1��$
��Left=�$��Bottom=�$��0,0,0,2q��� -:   $��
lblprauto2��$��2��$��2��$��Center=�$��Center=�$	��[%]���� -;   $��tfFittingAutoMaxXValue��$��4��$��50��$
��Left=�$��Center=��� -<   $��lblprautomaxXValue��$��4��$��Center=�$��Center=�$	��[%]��$��	61,3,93,7q�� -=   $��
gridpoint3��$��21��$��1�� �� $	��175 ��� $��30 �$��30���� $
��Auto ��� $��10 �$��10���� $	��175 �.+��� ->   $��stylePoint3��$��1��$��10��$��10��#��[  
���  ��� -?   $��gridAutoPoints3��$��2�� �� $��20 �$��20���� $��* ��� $��30 �$��30���� $��* ��� $��35 �$��35��.+��� -@   $��lblordnum3auto��$��0��$��Center=�$��Center=�$��T3���� -A   $��tffittingAutoProcent3��$��1��$
��Left=�$��Center=��� -B   $��
lblprauto3��$��2��$��2��$��Center=�$��Center=�$	��[%]��� -C   $��gridKeepOriginalData��$��22��$��1�� �� $	��155 ��� $��10 ��� $	��185 ��� $��10 ��� $	��175 �.+��� -D   $��chbShowOriginalData��$��2��$��10(�$
��Left=�$��Center=����� $��Zadrži originalan grafik��$
��Wrap=��� -E   $��lblPrelazniPeriod��$��24��$��1��$��20(�$
��Bold"�$��Center=�$��Center=�$��   Prelazni period   ��� -F   $��gridReHXRange��$��25��$��1�� �� $��10 �$��10���� $��* ��� $��10 �$��10���� $��* ��� $��10 �$��10���� $��* ��� $��10 �$��10���� $��* ��� $��10 �$��10��.+��� -G   $��lblReHXRange��$��1��$��3��$
��Left=�$��Center=�$
��Wrap=�$/��)Podrucje tecenja (ReL-a,ReH-a i Rp02-a)  ��$	��0,1q�$	��238���� -H   $��tfReHXRange��$��5��$��Stretch=�$��25��$
��Auto���� -I   $��lblReHXRangeEnd��$��7��$
��Left=�$��Center=�$	��[%]��� -J   $*��$gridRationReLRpmaxXrelXmaxForOnlyReL��$��27��$��1�� �� $��10 �$��10���� $��* ��� $��10 �$��10���� $��* ��� $��10 �$��10���� $��* ��� $��10 �$��10���� $��* ��� $��10 �$��10��.+��� -K   $!��lblRationReLRpmaxForOnlyReL��$��1��$
��Left=�$��Center=�$
��Wrap=�$$��(Max napon za ReL)/(Rm) [%]:  ��$
��0,-7q�$	��113���� -L   $ ��tfRationReLRpmaxForOnlyReL��$��3��$��Stretch=�$��25��$
��Auto���� -M   $!��lblRationXReLXmaxForOnlyReL��$��5��$
��Left=�$��Center=�$
��Wrap=�$��(Min AReL) [%]:���� -N   $��tfXReLForOnlyReL��$��7��$��Stretch=�$��25��$
��Auto��� -O   $��gridYieldOptionsOnlyReL��$��29��$��1�� �� $��10 �$��10���� $��* ��� $��10 �$��10���� $��* ��� $��10 �$��10���� $��* ��� $��10 �$��10���� $��* ��� $��10 �$��10��.+��� -P   $��lblOnlyReLPreasureUnit��$��1��$��3��$
��Left=�$��Center=�$
��Wrap=�$4��.Oscilacije napona za odredjivanje samo ReL-a  ��$	��0,1q�$	��238���� -Q   $��tfOnlyReLPreassureUnit��$��5��$��Stretch=�$��25��$
��Auto���� -R   $��lblOnlyReLReassureUnitEnd��$��7��$
��Left=�$��Center=�$��[MPa]��� -S   $��gridYieldOptions��$��31��$��1�� �� $��10 �$��10���� $��* ��� $��10 �$��10���� $��* ��� $��10 �$��10���� $��* ��� $��10 �$��10���� $��* ��� $��10 �$��10��.+��� -T   $��lblYield��$��1��$
��Left=�$��Center=�$
��Wrap=�$"��Min pad u [MPa] za [ReH] :  ���� -U   $��tfYield��$��3��$��Stretch=�$��25��$
��Auto���� -V   $��lblMinSubsReLFReL��$��5��$
��Left=�$��Center=�$
��Wrap=�$*��$Max razlika u [MPa] [ReLF] i [ReL] :���� -W   $��tfMaxSubsReLFReL��$��7��$��Stretch=�$��25��$
��Auto���� -X   $��
lblTearing��$��34��$��1��$��20(�$
��Bold"�$��Center=�$��Center=�$��   Kidanje   ��� -Y   $��gridForDetectFalseTearing��$��35��$��1�� �� $	��150 �$	��150���� $
��Auto ��� $��50 �$��50��.+��� -Z   $ ��lblTearingMinFallPreassure��$
��Left=�$��Center=�$
��Wrap=�$!��Min pad napona  kod kidanja��$��6,0,14,0q�$	��136���� -[   $��tfTearingMinFallPreassure��$��1��$
��Left=�$��Center=��� -\   $#��lblTearingMinFallPreassureEnd��$��2��$��Center=�$��Center=�$��[MPa]��� -]   $!��gridPodrazumevaniModkidanja��$��36��$��1�� �� $��3* ��� $��10 �$��10���� $
��1.5* ��� $��10 �$��10���� $��2* ��� $��10 �$��10��.+��� $��Right=�$��Center=�$
��Wrap=�$4��.Podrazumevani napon kidanja u odnosu na Rm :  ��$	��0,1q�$	��221���� -^   $!��tfDefaultPreassureOfTearing��$��2��$��0��$��Stretch=�$��25��$
��Auto���� -_   $%��lblDefaultPreassureOfTearingEnd��$��4��$��0��$
��Left=�$��Center=�$��
[procenat]��� -`   $&�� gridChbNepodrazumevaniModkidanja��$��38��$��1�� �� $	��260 ��� $��10 ��� $��10 ��� $��* �.+��� -a   $"��chbNepodrazumevaniModkidanja��$*��$Uključi nepodrazumevani mod kidanja��$��Center=�$��Center=��� -b   $��tblOpsegRacunanja��$��Opseg računanja n-a : ��$��3��� -c   $��gridResolutionForTearing��$��39��$��1�� �� $	��150 �$	��150���� $
��Auto ��� $��95 ��� $��10 ��� $��10 ��� $��* ��� $��85 �$��85���� $��40 �.+��� -d   $��lblResolutionForTearing��$��0��$��Right=�$��Center=�$ ��Rezolucija pri kidanju :  ���� -e   $��tfResolutionForTearing��$��1��$��26��$	��Top=��� -f   $ ��lblResolutionForTearingEnd��$��2��$��Center=�$��Center=�$��[ceo broj]    ���� -g   $��lblBeginIntervalForN��$��5��$
��Left=�$��Center=�$��Početak opsega :  ���� -h   $��tfBeginIntervalForN��$��6��$��0��$��Stretch=�$��25��$
��Auto���� -i   $��lblbeginterval��$��7��$��Center=�$��Center=�$	��[%]��� -j   $��gridTearingPoint��$��40��$��1�� �� $	��150 �$	��150���� $
��Auto ��� $��95 ��� $��10 ��� $��10 ��� $��* ��� $��85 �$��85���� $��40 �.+��� -k   $��lblTearingPointCoeff��$��0��$��Right=�$��Center=�$��Koeficijent kidanja :  ���� -l   $��tfTearingPointCoeff��$��1��$
��Left=�$��Center=��� -m   $��lblTearingPointCoeffEnd��$��2��$��Center=�$��Center=�$��[realan broj]���� -n   $��lblEndIntervalForN��$��5��$
��Left=�$��Center=�$��Kraj opsega :  ���� -o   $��tfEndIntervalForN��$��6��$��0��$��Stretch=�$��25��$
��Auto���� -p   $��lblendterval��$��7��$��Center=�$��Center=�$	��[%]��