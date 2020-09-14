# AP_Install_Generator
用於產生樹梅派WIFI發送設定腳本

##使用方法
1 - 啟動程式

2 - 輸入要設定的AP名稱密碼以及固定IP

3 - 點選"generate"並選擇存放位置生成

4 - 使用vncViewer或隨身碟將生成的"install.sh"存放至樹梅派中(建議存放至/home/pi/)

5 - 使用命令使系統能夠辨識其為執行檔(範例為存放於/home/pi/，若存放位置不同需更改)
    
    sudo chmod +x /home/pi/install.sh

6 - 輸入命令使用權限執行(範例為存放於/home/pi/，若存放位置不同需更改)

    sudo /home/pi/install.sh

7 - 完成後重新啟動樹梅派

8 - 連接樹梅派發送wifi後使用vncViwer連線至設定的固定IP

* - 腳本執行完成後將會中斷已連線的wifi，如果是讓樹苺派使用wifi遠端操控的話，中斷連線後約等待3~5分鐘或使用其他裝置搜尋已有發送訊號後就可以重新啟動了
