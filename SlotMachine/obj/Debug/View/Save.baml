   M S B A M L   `   `   `  ����F  BSlotMachine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null?$clr-namespace:SlotMachine.ViewModelsSlotMachine.ViewModels  5   	       SlotMachine.View.Save   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   3vm$clr-namespace:SlotMachine.ViewModels     5   	     � Title$
  Save��5   	   $	��272��5	   	   $	��354��5
   	    � WindowStartupLocation$ CenterScreen=�5   	   ���	#FF1E1E1E � 	Resources 5       �� 
TargetType �� Property �� Value �� 
TargetType �� CornerRadius��%  '��       �� 5   
   " ��6   .�5      ��  � �� �	#FF4444446+   �� 5       � ��
 �����6+   �� 5       � �� �	#FF6666666,   �� 5       � ��$ 1q�60   �� 5       � ��$ 10q�6(   �� 5       � ��$ 14(�6)   �� 5       � ��$
 Bold"�6+   �� 5       �@�Cursor$
 Hand��6'   �� 5       � �� 5      �� 5      " ��6&   .R�5      �� #��z��6!   #��z��5   !   #��z��5   !   $ 5��5   !   ._�5       �� $��Center=�6/   $��Center=�5!   /   5"      5#      5%      �� 5&       �@�OverridesDefaultStyle . 66   5'      5(      .��5*      � $��20q�6    � RowDefinitions 5+   
   �� 5,      $��* �6   �� 5-      $
��Auto �6   �� 5.      $
��Auto �6   .+�52   
   �� $��0��6   ���	#FF444444
����������	#FF666666$��1q�57      $��5q�58      $��14(�59      $��0,0,0,5q�5:      ��53      �� *	SaveFiles+5:      �� 5=   
   -   5        	 ��Name$	 txtPrice��5=      $��1��5?      $��0,0,0,15q�5@      ���	#FF444444
����������	#FF666666$��1q�5D      $��5q�5E      $��14(�5F      ��5>      �� *SaveFileName+
 �� UpdateSourceTrigger$
 PropertyChanged=�5F      �� 5H   
   $��2��6    �� Orientation$ 
Horizontal=�6"   $��Center=�6;   .+�5I      �� $
��Save��6   $	��120��5K      $��0,0,10,0q�5L      ��5J      �� *SaveCommand+5L      �� 5M      $��Cancel��6   $	��120��5O      ��5N      �� *CancelCommand+5O      5P      5Q      5R      