   M S B A M L   `   `   `  ����U  QSocketPrograming_ClientWpf, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullG(clr-namespace:SocketPrograming_ClientWpfSocketPrograming_ClientWpf  5   	   +    %SocketPrograming_ClientWpf.MainWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   :local(clr-namespace:SocketPrograming_ClientWpf     5   	     � Title$  Client��5   	   $	��400��6   $	��600��6%   ��5	      �� 5
   
    �� ImageSource$. (/Image/Win11-background-3-min-scaled.jpg��6   .��5      � ��5   
   �� 5      $- 'Image\Win11-background-3-min-scaled.jpg��6   .+�5   
   �� $��10q�6   $	��100��6!   $
��Left=�6-   .+�5      �� $	��IP:��6   
�������6#   �� 5      -   5        $��txtIP��5      $��	127.0.0.1��6#   �� 5      $��Port:��6   $��0,5,0,0q�6%   
�������66   �� 5      -   5        $��txtPort��5      $
��8080��6%   �� 5      -   5        $��
btnConnect��5   *   $��
Connecting��6   $��0,10,0,0q�6<   
���������6g   Z� 6~   �� 5      -   5        $��btnExit��5   $   $
��Exit��6   $��0,10,0,0q�63   
���������6o   Z� 6�   �� 5      -   5        $��btnSend��5   $   $
��Send��6   $��0,10,0,0q�63   
���������6o   Z� �� 5      -   5         ��Name$ 
btnPrivate��5      $��پیام خصوصی��6)   $	��100��6>   
�������$��0,10,0,0q�5   '   ��6:   Z� 5      �� 5   
   $	��400��6   $��Right=�6!   .+�5       �� $��Center=�6   $��Client��65   
�������$��30(�6V   $��	20,20,0,0q�6d   6y   �� 5#      $��Server Response :��6   $��	0,10,30,0q�61   
�������$
��Bold"�6W   $��Right=�6i   �� 5$      -   5        $��txtChatMessage��5$      $	��200��6-   $	��356��6:   $��Right=�6G   
�������$��	10,0,30,0q�6�   ��6c   Z� 6�   �� 5%      $��	Messahe :��6   $��
20,20,20,0q�6)   
�������6=   �� 5&      -   5        $��
txtMessage��5&      $��
20,10,20,0q�6)   5'      5)      5*      