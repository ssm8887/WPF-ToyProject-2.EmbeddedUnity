# ToyProject
## 2. WPF내 Unity 내장 및 TCP통신 구현

+ ### 기간
	+ 20/11/30 ~ 20/12/11

+ ### 목적
	+ 훈련장비 모의시 Unity에서 구현하기 어려운 부분(3D 모델 동작화면)을 제외한 인터페이스를  
	Unity로	제작하지 않기위함.

+ ### 구현기능
	+ WPF 윈도우 내 Unity화면 내장
	+ TCP / Named Pipe 방식으로 버튼과 상호작용

+ ### 참조 URL
	+ Unity Embed 참고  
	https://stackoverflow.com/questions/44059182/embed-unity3d-app-inside-wpf-application

	+ WPF Handle 관련 내용  
	https://www.codeproject.com/Questions/750719/what-is-the-alternative-for-csharp-panel-handle-in

	+ C# TCP Server/Client  
	http://www.csharpstudy.com/net/article/6-%eb%b9%84%eb%8f%99%ea%b8%b0-TCP-%ec%84%9c%eb%b2%84  
	http://www.csharpstudy.com/net/article/4-TCP-%ed%81%b4%eb%9d%bc%ec%9d%b4%ec%96%b8%ed%8a%b8

	+ Named Pipes Stream  
	https://seonbicode.tistory.com/6

+ ### 구동 화면

| 초기 실행화면		 | Unity -> WPF			 |
| ------------ 		| ------------			|
| ![image.png]() 	| ![image.png]() 		|

| WPF -> Unity 		| 						|
| ------------ 		| ------------ 			|
| ![image.png]() 	|  						|

+ ### 소스코드
	+ Git – https://github.com/ssm8887/WPF-ToyProject-2.EmbeddedUnity
	+ Yobi - http://ms-filesvr:9000/yobi/ssm8887/ToyProject-2.EmbeddedUnityInWPF
