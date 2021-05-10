
# ET_NetcodeDemo 
Demo for netcode,develop with ET
# 设计思路
## 1.模仿守望先锋分享的流程(同步方式是命令帧+状态),但是以MOBA来做先. (后面俯视角的做完了,再开个分支做第三人称过肩视角的TPS)  
## 2.客户端16ms帧间隔,服务器48ms. 这样服务器一秒差不多20帧,客户端一秒60帧. 客户端存在3个时间点:
  * 2.1 最后收到的服务器时间点. 也就是服务器更新每帧消息时,转换为的客户端帧数
  * 2.2 当前为整个世界内非主角单位插值用的模拟时间点. 一般会比服务器时间点慢6帧(也就是服务器上的2帧)
  * 2.3 主角单位自身模拟的时间点. 一般为最后确认的时间点+RTT+3 (这个3代表服务器上的1帧)
## 3.客户端每帧流程->  
  * 3.1 先更新服务器下发的各种世界状态变化 (主角自己的不做处理,走预测模拟那套)  
  * 3.2 检测是否可以继续模拟(客户端模拟时间相对已经确定的帧,不会超过250ms)  
  * 3.3 如果可以继续模拟下一帧,就模拟帧+1检测自己的输入,发送输入给服务器(如果有的话)
## 4.服务器每帧流程->
  * 4.1 帧步进,先收集各个输入,对世界状态进行一定的修改.    
  * 4.2 驱动世界刷新  
  * 4.3 遍历所有单位状态,计算每个玩家需要更新的帧数据增量.  
  * 4.4 遍历所有玩家,下发对应的帧数据增量
 
   
# TODO
1.测试丢包率10%,延迟10-750ms左右的表现,并进行优化,修复可能存在的问题.顺便验证目前的实现是否合理. (目前单独测试延迟10-300ms时表现还行,超过300,其他人的移动就出现卡顿感以及看起来超出预期的时间滞后.需要进一步研究处理)  
2.目前是MOBA视角,客户端自己预测的时候用的是自身寻路,服务器让这个单位行走的时候用的是服务器的寻路. 有可能因为各种误差导致得到的路径不一致.可以考虑改为客户端寻路,服务器验证路径后直接用这个路径  
3.需要支持单位移动速度突变  
4.加入战斗元素,做一下简单的PVP. (有造成伤害,死亡,复活流程)  

https://user-images.githubusercontent.com/29506629/117576578-13d5a780-b119-11eb-805c-9fccdba5d29a.mp4
