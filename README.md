# FrameSync  

欢迎交流和补充优化。  
--
这只是个帧同步的demo，所以断线处理什么的都没有做，不在这个demo要呈现的范围内  
本demo适合2d的逻辑计算

FrameServer是服务端的demo，用unity做的  
FrameServer/Assets/Scripts/MarsNet/ServerConfig.cs里面的battleUserNum参数是配置一场战斗需要几个人参与

FrameClient是客户端的demo  
有做的:  
1.一致性随机数  
2.自己写的一些简单碰撞  
3.一个简单的游戏demo，只是可以发射子弹破坏障碍，如果需要攻击角色的可以自己扩展下  

没有做的(功能比较简单，需要的自己实现就好了)  
抗udp丢包：使用冗余包，具体冗余包几个，需要检测网络状况来进行动态调整，比较省流量  
抗网络抖动：使用后模拟，不建议使用帧缓存，帧缓存会加大操作延迟  

定点数物理引擎:
Box2DSharp:https://github.com/jeason1997/Box2DSharp   C#版Box2D物理引擎，github上有一个定点数分支
一个基于C#的3D定点数物理引擎：https://github.com/sam-vdp/bepuphysics1int
