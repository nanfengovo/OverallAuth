   M S B A M L   `   `   `  ����I  EOverallAuthWPF, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null[2clr-namespace:OverallAuthWPF.Additional_properties$OverallAuthWPF.Additional_properties  5      =#clr-namespace:OveralllAuth_V1.ViewsOveralllAuth_V1.Views  5      "    OveralllAuth_V1.Views.UserUC   5      W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      SadditionalProperties2clr-namespace:OverallAuthWPF.Additional_properties     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      5local#clr-namespace:OveralllAuth_V1.Views     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5        � 	Resources  5         DynamicPlaceholderStyle �� 
TargetType �� Property �� Value �� 
TargetType �� Path �� RelativeSource��%�   &	        �� 5   
   " ��60   .�5      ��  � �� 5      �� 5      " ��6&   .R�5      �� 
���������64   �� $: 4(additionalProperties:TextBoxHelper.PlaceholderText)� �� *TemplatedParent+5      5      5      5      .��5      �� 
������  �� CornerRadius$ 	0 10 10 0��6&   ._�5   
   �  � RowDefinitions 5      �� 5      $
��Auto �6    �� 5      .+�5       �� $
��80px��5!      $��5,20,5,5q�5"      
�������$ 20��5$      ._�5%      � 	 � ColumnDefinitions	 5&      �� 5'      $��* �6+   �� 5(      $��* �6+   �� 5)      .+�5,      �� $��60,20,20,20q�5-      $��Center=�5.      
 �� Orientation$
 
Horizontal=�5/      .+�50      �� 用户名：6*   �� 51      $	��280��52      8   2OverallAuthWPF.Additional_properties.TextBoxHelper   PlaceholderText$$ 请输入需要搜索的用户��53      #��[  54      55      �� 57      $��1��58      $��60,20,20,20q�59      $��Center=�5:      $
 
Horizontal=�5;      .+�5<      �� 	角色：6)   �� 5=      $	��280��5>      $$ 请输入需要搜索的角色��5?      #��[  5@      5A      �� 5C      $��2��5D      $��60,20,20,20q�5E      $��Center=�5F      $
 
Horizontal=�5G      .+�5I      �� $	��120��5J      $��40��5K      $��0,0,20,0q�5L      
����� �$ 10��5N      ._�5O      �� 
������ $��0q�6>   .��6R   搜索5Q      5R      �� 5T      $	��120��5U      $��40��5V      
�������$ 10��5X      ._�5Y      �� 
������ $��0q�6>   .��6R   取消5[      5\      5]      5^      5_      �� 5b      $��1��5c      $��5,20,5,5q�5d      
�������$ 20��5f      ._�5h      �  5i      �� 5j      $
��Auto �6(   �� 5k      $
��Auto �6(   �� 5l      $��60 �6(   .+�5p      �� $��0��5q      $��40,30q�5r      $
��Left=�5s      $
 
Horizontal=�5t      .+�5v      �� $	��120��5w      $��40��5x      $��0,0,10,0q�5y      ���#409eff$ 10��5{      ._�5|      �� 
������ $��0q�5~   !   $��刷新��5�   !   $��16(�5�   !   
���������5   !   �� *LoadDataCommand+5�   !   5�      �� 5�      $	��120��5�      $��40��5�      $��0,0,10,0q�5�      ���#f56c6c$ 10��5�      ._�5�      �� 
������ $��0q�5�   !   $��删除��5�   !   $��16(�5�   !   
�������5�   !   5�      5�      �� 5�      $��0��5�      $	��120��5�      $��40��5�      $��0,0,50,0q�5�      $��Right=�5�      ���#409eff$ 10��5�      ._�5�      �� 
������ $��0q�5�      $��+ 添加用户��5�      $��16(�5�      
�������5�      5�      &   System.Windows.Controls.DataGrid  5�      $��1��5�         AlternatingRowBackground �#f0f0f0   AutoGenerateColumns$ False��5�         CanUserAddRows$ False��5�         CanUserDeleteRows$ False��5�         ColumnHeaderHeight$ 40Q�5�         EnableRowVirtualization$
 True��5�         GridLinesVisibility$	 All=�5�         HeadersVisibility$ Column=�5�         HorizontalGridLinesBrush
 �����   
IsReadOnly$
 True��5�         SelectionMode$ Single=�5�         SelectionUnit$ FullRow=�5�         VerticalGridLinesBrush
 �������5�      �� *Users+   Columns 5�      4  .System.Windows.Controls.DataGridCheckBoxColumn  5�      ,  &System.Windows.Controls.DataGridColumn   Width5  /System.Windows.Controls.DataGridLengthConverter$
 Auto 65      Header$ 全选��6B      HeaderTemplate 5�   "   �� 5�   &   .R�5�   *   �� $��Center=�5�   -   $��全选��5�   -    d� 	IsChecked 5�   -   �� *DataContext.IsAllSelected+ ��  �� AncestorType DataGrid5�   '   1  +System.Windows.Controls.DataGridBoundColumn   Binding 5�   "   �� 5�   &     �� Mode$  TwoWay=�6.   $ 
IsSelected�6<   5�      0  *System.Windows.Controls.DataGridTextColumn  5�      $
 Auto 5�   !   $ 序号��5�   !   !   
IsReadOnly$
! True��5�   !    5�   !   �� *Index+5�   !   4  .System.Windows.Controls.DataGridTemplateColumn  5�      $
 Auto 65   $ 操作��6B   "   CellTemplate" 5�   "   �� 5�   &   .R�5�   *   �� $
 
Horizontal=�65   .+�5�   .   �� $��5q�5�   1   ���#409eff$ 5��5�   1   ._�5�   2   �� $��2q�5�   5   $��5q�5�   5   
������ $��0q�5�   5   $��编辑��5�   5   ��5�   5   �� *EditCommand+5�   5   5�   /   �� 5�   .   $��5q�5�   1   ���#f56c6c$ 5��5�   1   ._�5�   2   �� $��2q�5�   5   $��5q�5�   5   
������ $��0q�5�   5   $��删除��5�   5   ��5�   5   �� *DeleteCommand+5�   5   5�   /   5�   +   5�   '   5�        5�      $ * 5�   !   $ 	用户名��5�   !    5�   !   �� *Name+5�   !     5�      $ * 5�   !   $ 角色��5�   !    5�   !   �� *
RoleName+5�   !     5�      $ * 5�   !   $ 描述��5�   !    5�   !   �� *Description+5�   !     5      $
 Auto 5  !   $ 是否启用��5  !    5  !   �� *	IsEnabled+5  !     5     $ * 61   $ 创建时间��6;    5  "   �� 5  &   $ 
CreateTime�6.   # �� StringFormat$# yyyy-MM-dd HH:mm:ss��6@   5
       5     $ * 61   $ 更新时间��6;    5  "   �� 5  &   $ 
UpdateTime�6.   $# yyyy-MM-dd HH:mm:ss��6@   5     $   ColumnHeaderStyle$ 5     �� 5     =	  7System.Windows.Controls.Primitives.DataGridColumnHeader" 	 6$   .�5  "   ��  � ��$ Center=�6O   �� 5  "    � �� �#e0e0e06?   �� 5  "    � ��$
 Bold"�6?   5     5     5     5      5!     5"     5#     