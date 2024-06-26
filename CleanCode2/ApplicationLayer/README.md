# 2. Уровень классов.

### 2.1. Класс слишком большой (нарушение SRP), или в программе создаётся слишком много его инстансов (подумайте, почему это плохой признак).

Большой класс (например, [Car](Car.cs#4)) очень сложно поддерживать и тестировать, что ведет к образованию множества ошибок. При таком подходе очень легко сломать существующую функциональность, случайно задев что-то в большом классе.

Много инстансов одного класса могут сильно перегрузить проект, легко получить анти-паттерн God object. Такой проект сложно сделать модульным, а значит мы сильно ограничены в гибкости и получаем очень сильную связанность. 

Так может получиться:
1. При старте проекта, когда сложно представить из каких модулей система должна состоять и все начинается с одного класса в надежде, что со временем произведем рефактор. 
2. Когда непонятно какой класс ответственный за функциональность. Так называемые Utils-классы, в которые помещают все что угодно. 

### 2.2. Класс слишком маленький или делает слишком мало.

В противопоставлении большим классам, когда сложно спроектировать хорошую модульность, иногда начинают дробить на небольшие классы все подряд, полагая, что этот подход лучше, чем организация кода в большом классе. 

Очевидно, все зависит от ситуации, но много маленьких классов с небольшой зоной ответственности может быть так же плохо для проекта, как и "божественные классы". 

При наличии множества классов проект запутывается очень быстро: разработчики не понимают как взаимодействуют классы между собой и это создает большие трудности при добавлении нового функционала. И это еще не говоря про рисование диаграмм классов. 

### 2.3. В классе есть метод, который выглядит более подходящим для другого класса.

С ростом проекта может случиться ситуация, когда у конкретного класса появился метод больше подходящий для другого класса, что может свидетельствовать о плохом дизайне приложения. Например в [Car](Car.cs#4) методы `StartEngine`, `Accelerate`, `Brake` могут быть вынесены в отдельный класс или интерфейс, чтобы их было проще переиспользовать. 

### 2.4. Класс хранит данные, которые загоняются в него в множестве разных мест в программе.

Классическая проблема "божественного класса", когда класс хранит не сколько большие методы, а множество состояний или данных, которые меняются из множества разных мест.

Например, класс запроса, который сквозным образом идет через весь проект, попутно наполняясь информацией.

Зачастую так делается для простоты, чтобы "не множить сущности" и не прокидывать очередную DTO по всему проекту или разным модулям. 

### 2.5. Класс зависит от деталей реализации других классов.

В классе [Car](Car.cs#4) с наследниками `ElectricCar` и `GasolineCar` можно заметить, как все производные зависят от реализации базового класса. У классов есть общие методы, которые наследники не переопределяют, что делает дизайн довольно хрупким при изменениях.

Такое происходит, когда в ходе развития проекта появилась потребность в новых типах или фичах, не свойственных родительскому классу, но при этом очень на него похожих, что дает ложную уверенность в правильности завязки именно на такую иерархию. 

### 2.6. Приведение типов вниз по иерархии (родительские классы приводятся к дочерним).

Множественное использование виртуальных методов может вызывать такое состояние в приложении, когда все новые фичи находятся в переопределенных классах наследников. Создается очень тесно связанная система при таком типе приведения, которую сложно расширять без изменения всего остального. 

### 2.7. Когда создаётся класс-наследник для какого-то класса, приходится создавать классы-наследники и для некоторых других классов.

Для взаимодействия классов между собой при создании новой функциональности возможны ситуации, когда 2 класса-родителя непосредственно друг с другом не общаются, но их наследники могут это делать. 

### 2.8. Дочерние классы не используют методы и атрибуты родительских классов, или переопределяют родительские методы.

В такую ситуацию мы попадаем, когда в наследуемых классах мы переопределяем поведение родительского класса и начинаем создавать классы-специализации. Мы теряем изначальную функциональность родительского класса и начинаем теряться в множественной иерархии различных наследников, которые переопределяют друг друга для добавления новой фичи.  

# 3. Уровень приложения.

### 3.1. Одна модификация требует внесения изменений в несколько классов.

Если мы наследуемся от абстракного класса или завязались на общий интерфейс, который затем различные классы реализует, то легко получить ситуацию, когда изменения в корневом абстрактном классе или интерфейсе влекут за собой череду изменений по всей иерархии проекта.

### 3.2. Использование сложных паттернов проектирования там, где можно использовать более простой и незамысловатый дизайн.

Неразумное использование паттернов проектирования, легко приводит к описанной ситуации. В профессиональной разработке нет ничего универсального, каждая фича и каждый кейс зависит от отдельного бизнес-требования в конкретной среде с определенными параметрами. Очевидно, можно слепо попробовать следовать лучшим практикам и добавлять в свой проект множество избыточного кода, чтобы соблюсти дизайн лучших паттернов проектирования, но зачастую такое поведение - избыточно. 

Всегда важно отталкиваться от своего окружения, продуктовых требований и команды разработки, чтобы адаптировать лучшие практики и паттерны конкретно под своей кейс, а не переиспользовать 1 в 1, не задумываясь, потеряв возможность для гибкого изменения в последствии и сложной поддержки на длительном сроке. 