   M S B A M L   `   `   `  ����F  BSlotMachine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:SlotMachine.ViewSlotMachine.View  5   	   ?$clr-namespace:SlotMachine.ViewModelsSlotMachine.ViewModels  5   	       SlotMachine.View.Load   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   0localclr-namespace:SlotMachine.View     5   	   3vm$clr-namespace:SlotMachine.ViewModels     5   	     � Title$
  Save��5	   	   $	��272��5
   	   $	��354��5   	    � WindowStartupLocation$ CenterScreen=�5   	   ���	#FF1E1E1E � 	Resources 5       �� 
TargetType �� Property �� Value �� 
TargetType �� CornerRadius��%  '��       �� 5   
   " ��6   .�5      ��  � �� �	#FF4444446+   �� 5       � ��
 �����6+   �� 5       � �� �	#FF6666666,   �� 5       � ��$ 1q�60   �� 5       � ��$ 10q�6(   �� 5       � ��$ 14(�6)   �� 5       � ��$
 Bold"�6+   �� 5       �@�Cursor$
 Hand��6'   �� 5       � �� 5       �� 5!      " ��6&   .R�5"      �� #��z��6!   #��z��5#   !   #��z��5$   !   $ 5��5%   !   ._�5&      �� $��Center=�6/   $��Center=�5'   /   5(      5)      5+      �� 5,       �@�OverridesDefaultStyle . 66   5-      5.      .��52      � $��20q�6    � RowDefinitions 53   
   �� 54      $��* �6   �� 55      $
��Auto �6   .+�58   
   �� $��0��6   ���	#FF444444
����������	#FF666666$��1q�6�   $��5q�6�   $��14(�6�   $
��auto��6�   $��0,0,0,5q�6�   ��6   �� *	SaveFiles+	 �� SelectedItem	 6A   �� *SelectedSaveFile+6�   �� 5:   
   $��1��6   
 �� Orientation$
 
Horizontal=�6"   $��Center=�6;   .+�5;      �� $
��Load��6   $	��120��6D   $��0,0,10,0q�6P   ��6$   �� *LoadCommand+6P   �� 5<      $��Cancel��6   $	��120��6I   ��6&   �� *CancelCommand+6I   5=      5>      5?      