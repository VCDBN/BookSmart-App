   M S B A M L   `   `   `  ����a  ]GongSolutions.WPF.DragDrop, Version=3.0.0.0, Culture=neutral, PublicKeyToken=91f1945125b7a587kLclr-namespace:GongSolutions.Wpf.DragDrop;assembly=GongSolutions.Wpf.DragDropGongSolutions.Wpf.DragDrop  5      D @BookSmart, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:BookSmart.PagesBookSmart.Pages 5      (   "BookSmart.Pages.ReplacingBooksPage   5      W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      [ddLclr-namespace:GongSolutions.Wpf.DragDrop;assembly=GongSolutions.Wpf.DragDrop     5      /localclr-namespace:BookSmart.Pages     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      5ui&http://schemas.lepo.co/wpfui/2022/xaml     5	        N� Title$  ReplacingBooksPage��5
       N� 
Background �#191919 � 	Resources 5       �� 
TargetType �� Property �� Value �� 
TargetType ��Name �� CornerRadius	 �� Triggers
 P� Property P� Value �� 
TargetName   CustomTextBoxStyle��%u  '��       &	  �    �� 5   
   " ��6   .�5      ��  � ��
 �����6+   �� 5       � ��$	 150��6&   �� 5       � ��$ 35��6'   �� 5       � ��$ 10q�6'   �� 5       � ��$ 1q�60   �� 5       � ��$ Segoe UI)�6+   �� 5       � ��$ 14(�6)   �� 5       � ��$ Regular"�6+   �� 5       � �� 5      �� 5      " ��6&   .R�5      �� $ border��5      ���#303030$ 5��5!      ._�5"      �� $��Center=�6/   $��Center=�6L   5#      	 5$      P� 5%      
 �@J�IsMouseOver$
 True��6=   . �5&   "   �� $ border��6)    � �� �#5050506S   5'      5)      5+      5,      �� 5/   
   " ��6+   .�50      ��  � ��
 �����6+   �� 51       � ��$ Segoe UI)�6+   �� 52       � ��$ 20(�6)   �� 53       � ��$ Regular"�6+   �� 54       �@��Padding$ 10q�6(   �� 55       � ��$ 0q�6'   �� 56       �@��TextAlignment$
 Left=�6.   �� 57       � ��$	 150��6&   �� 58       � ��$
 Left=�64   �� 59       � ��$ Center=�62   5:      5;      .��5=      � .+�5>   
   �� $��Center=�5?      $��Center=�5@       �� Orientation$ 
Horizontal=�5A      .+�5B      �� $��	0,0,120,0q�5C      $
��Left=�5D      $	��Top=�5E      $ Vertical=�5F      .+�5G      �� -   5        $ ��lblReplacingBooksPageTitle��5H      $
��Left=�5I      $	��Top=�5J      $��Replacing Books��5K      $��30(�5L      $��DemiBold"�5M      
�������5N      �� 5O      -   5        $��lblInfo��5P      $	��260��5Q      $��
10,30,0,20q�5R      $
��Left=�5S      
�������$��
Loading...��5U       �� TextAlignment$
 Left=�5V      $
��Wrap=�5W      �� 5X      -   5        $��btnExit��5Y      $
��Left=�5Z      $��Back to Menu��5\      5]      �� 5_      $��Right=�5`      $��Center=�5a      $ 
Horizontal=�5b      .+�5c      � -   5         �Name$ pbRemainingTime��5d      $��5��5e      $	��240��5f      $��Center=�5g      ���#191919
���2�2� � Maximum$ 60Q�5j       � Orientation$ Vertical=�5k       � Value$ 60Q�5l       � LayoutTransform 5m      �� 5n       �� ScaleY$ 1Q�6)   5p      �� 5r      $��Center=�5s      $��Center=�5t      $ Vertical=�5u      .+�5v      �� -   5         ��Name$ tbTimer��5w      $��	0,10,0,10q�5x      $��30(�5y      
���2�2�$��01:00��5{      $ Center=�5|      �� 5}      -   5         ��Name$ lbCallNumbers��5~      $	��150��5      $	��260��5�      $��Center=�5�      )   #GongSolutions.Wpf.DragDrop.DragDrop   IsDragSource$
 True��5�         IsDropTarget$
 True��5�      ���#191919
�������$��2q�5�      $��16(�5�      
�������   DropHandler 5�      �� ��5�      �� *CallNumbers+ 5�      ��%_   '��       �� 5�      " ��6$   .�5�   "   ��  �@��CornerRadius$ 10��6A   5�      5�      5�      �� 5�      -   5         ��Name$ 	btnSubmit��5�      $
��Left=�5�      $��Submit��5�      ${�false��5�      5�      �� 5�      $��20,0,0,0q�5�      $��Center=�5�      $��Center=�5�      $ Vertical=�5�      .+�5�      �� -   5        $ btnStart��5�      $
��Left=�5�      $��New Game��5�      �� 5�      -	   5        $ tbNumSolved��5�      #��[  5�      $��	Solved: 0��5�      �� 5�      -
   5        $ tbScore��5�      #��[  5�      $��Score: 0��5�      5�      5�      5�      5�      5�      