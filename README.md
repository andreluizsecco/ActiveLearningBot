# ActiveLearningBot
Chatbot utilizando o conceito de Active Learning com o Bot Framework v4 e o LUIS (Microsoft Cognitive Services)

## Active Learning
> "O aprendizado ativo é um caso especial de aprendizagem de máquina semi-supervisionada em que um algoritmo de aprendizagem é capaz de interagir com o usuário (ou alguma outra fonte de informação) para obter as saídas desejadas em novos pontos de dados." (Wikipedia)

## O Projeto
O projeto é um chatbot simples para exemplificar o conceito de active learning aplicado a bots com o uso do [LUIS](http://luis.ai).
Para tal, o mesmo faz uso do SDK [Cognitive.LUIS.Programmatic](https://www.nuget.org/packages/Cognitive.LUIS.Programmatic) que permite criar novas intenções, entidades, treinar seu modelo e publicá-lo de forma programática.

O SDK está disponível no Nuget e em um projeto Open Source no GitHub:
* [Cognitive.LUIS.Programmatic (Nuget)](https://www.nuget.org/packages/Cognitive.LUIS.Programmatic)
* [Projeto Open Source](https://github.com/andreluizsecco/Cognitive-LUIS-Programmatic)

## Getting started
* Importe o arquivo JSON da pasta **LuisApp** na sua conta no site [luis.ai](http://luis.ai);
* Abra o projeto na pasta **src** e o arquivo **appsettings.json**, preenchendo as chaves **{LUIS:AppId}**, **{LUIS:AuthoringKey}** e **{LUIS:APIHostName}** com o valor das suas respectivas chaves do seu App no LUIS.
