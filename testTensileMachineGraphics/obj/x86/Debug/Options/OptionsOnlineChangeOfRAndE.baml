   M S B A M L   `   `   `  ����  testTensileMachineGraphicsC    =testTensileMachineGraphics.Options.OptionsOnlineChangeOfRAndE   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   -   5          � 
ResizeMode$  NoResize=�5   	    � Title$0 *Opcije grafika promene napona i izduženja��5   	   $	��360��6;   $	��350��6H   .��5      � 
������� � 	Resources 5	   
    �� 
TargetType �� Property �� Value��%  '��       '	� y     '�� �     '�� o    �� 5
      " ��6   .�5      ��  � ��$ 15(�6.   �� 5       � ��$ Times New Roman)�6/   5      �� 5      " 	�6   .�5      ��  � ��$
 14.5(�6.   �� 5       � ��$ Times New Roman)�6/   5      �� 5      " ��6   .�5      ��  � ��$
 14.5(�6.   �� 5       � ��$ Times New Roman)�6/   5      �� 5      " ��6   .�5      ��  � ��
 �����60   �� 5       � ��
 �   �60   5      5       � RowDefinitions 5!   
   �� 5"      $��10 �6   �� 5#      $��35 �6   �� 5$      $��26 �6   �� 5%      $��26 �6   �� 5&      $��26 �6   �� 5'      $��10 �6   �� 5(      $��10 �6   �� 5)      $��10 �6   �� 5*      $��35 �6   �� 5+      $��26 �6   �� 5,      $��26 �6   �� 5-      $��26 �6   �� 5.      $��10 �6   .+�51   
   �� -   5        $#��grboxEpruvetaRazmeraChangeOfR��51      $$��Razmera grafika promene napona��52      $��15(�53      
���   �
���   �$��0��56      $��0��57      $��6��58      $��3��59      �� 5<   
   -   5        $,��&grboxEpruvetaRazmeraChangeOfElongation��5<      $(��"Razmera grafika promene izduženja��5=      $��15(�5>      
���   �
���   �$��7��5A      $��0��5B      $��6��5C      $��3��5D      �� 5F   
   -   5        $��lblGraphicRatioChangeOfR��5F      $��1��5G      $��1��5H      $��15(�5I      $
��Bold"�5J      $��Center=�5K      $��Center=�5L      $*��$   Razmera grafika promene napona   ��5M      � 5O   
   -   5        $��gridRbtnsRationChangeOfR��5O      $��2��6/   $��1��6<    � ColumnDefinitions 5P      �� 5Q      $��10 �6#   $��10��6.   �� 5R      $��2* �6#   �� 5S      $��10 �6#   $��10��6.   �� 5T      $
��1.6* �6#   �� 5U      $��10 �6#   $��10��6.   .+�5X      	� -   5        $��rbtnAutoChangeOfR��5_       	� 	GroupName$ razmeraChangeOfR��5X      $��automatsko podešavanje��5Y      $��1��5Z      $��16��5[      $
��Left=�5^      $��Center=�5`      	� 5a      -   5        $��rbtnManualChangeOfR��5h      $ razmeraChangeOfR��5a      $��ručno podešavanje��5b      $��16��5c      $
��Left=�5f      $��3��5g      $��Center=�5i      5k      � 5m   
   -   5        $��gridTfPreassureChangeOfR��5m      $��3��6/   $��1��6<    5n      �� 5o      $��10 �6#   $��10��6.   �� 5p      $
��1.5* �6#   �� 5q      $��10 �6#   $��10��6.   �� 5r      $
��1.5* �6#   �� 5s      $��10 �6#   $��10��6.   .+�5v      �� -	   5        $ ��lblRatioPreassureChangeOfR��5v      $��1��5w      $��Right=�5x      $��Center=�5y      $��Napon[MPa]  :  ��5z      �� 5}      -
   5        $��tfRatioForceChangeOfR��5~      $��3��5}      $��Stretch=�5�      $��25��5�      $
��Auto��5�      5�      � 5�   
   -   5        $��gridTfElongationChangeOfR��5�      $��4��60   $��1��6=    5�      �� 5�      $��10 �6#   $��10��6.   �� 5�      $
��1.5* �6#   �� 5�      $��10 �6#   $��10��6.   �� 5�      $
��1.5* �6#   �� 5�      $��10 �6#   $��10��6.   .+�5�      �� -   5        $��lblRatioTauChangeOfR��5�      $��1��5�      $��Right=�5�      $��Center=�5�      $��tau[s]  :  ��5�      �� 5�      -   5        $��tfRatioTauChangeOfR��5�      $��3��5�      $��Stretch=�5�      $��25��5�      $
��Auto��5�      5�      �� 5�   
   -   5        $'��!lblGraphicRatioChangeOfElongation��5�      $��8��5�      $��1��5�      $��15(�5�      $
��Bold"�5�      $��Center=�5�      $��Center=�5�      $.��(   Razmera grafika promene izduženja   ��5�      � 5�   
   -   5        $'��!gridRbtnsRationChangeOfElongation��5�      $��9��68   $��1��6E    5�      �� 5�      $��10 �6#   $��10��6.   �� 5�      $��2* �6#   �� 5�      $��10 �6#   $��10��6.   �� 5�      $
��1.6* �6#   �� 5�      $��10 �6#   $��10��6.   .+�5�      	� -   5        $ ��rbtnAutoChangeOfElongation��5�      $ razmeraChangeOfElongation��5�      $��automatsko podešavanje��5�      $��1��5�      $��16��5�      $
��Left=�5�      $��Center=�5�      	� 5�      -   5        $"��rbtnManualChangeOfElongation��5�      $ razmeraChangeOfElongation��5�      $��ručno podešavanje��5�      $��16��5�      $
��Left=�5�      $��3��5�      $��Center=�5�      5�      � 5�   
   -   5        $'��!gridTfPreassureChangeOfElongation��5�      $��10��68   $��1��6F    5�      �� 5�      $��10 �6#   $��10��6.   �� 5�      $
��1.5* �6#   �� 5�      $��10 �6#   $��10��6.   �� 5�      $
��1.5* �6#   �� 5�      $��10 �6#   $��10��6.   .+�5�      �� -   5        $*��$lblRatioElongationChangeOfElongation��5�      $��1��5�      $��Right=�5�      $��Center=�5�      $��Izduženje[%]  :  ��5�      �� 5�      -   5        $#��tfRatioElonChangeOfElongation��5�      $��3��5�      $��Stretch=�5�      $��25��5�      $
��Auto��5�      5�      � 5�   
   -   5        $(��"gridTfElongationChangeOfElongation��5�      $��11��69   $��1��6G    5�      �� 5�      $��10 �6#   $��10��6.   �� 5�      $
��1.5* �6#   �� 5�      $��10 �6#   $��10��6.   �� 5�      $
��1.5* �6#   �� 5�      $��10 �6#   $��10��6.   .+�5�      �� -   5        $#��lblRatioTauChangeOfElongation��5�      $��1��5�      $��Right=�5�      $��Center=�5�      $��tau[s]  :  ��5�      �� 5�      -   5        $"��tfRatioTauChangeOfElongation��5�      $��3��5�      $��Stretch=�5�      $��25��5�      $
��Auto��5�      5�      5�      5�      