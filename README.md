### Как установить
``` https://github.com/EveryGameSPlay/Package-Unity-Types.git ```

[Инструкция установки](https://docs.unity3d.com/Manual/upm-ui-giturl.html)
## Общее
Данный пакет Я решил собрать для оптимизации рабочего процесса. Он включает в себя базовые типы, используемые мной при разработке различных проектов.

И хоть на данный момент пакет содержит не так много новых типов, однако каждый из них решает определенную задачу. Кроме того, в пакет включены методы расширения для некоторых базовых типов!

```cs
Основные типы:
- Option<T>
- Promise<T>
- Singleton<T>
  
Расширения:
- Float
- Int
- Linq
- Collections
- Unity Objects
```

## Примеры использования
Далее приведены примеры использования базовых типов и некоторых расширений.  
### Option\<T\>
Данный тип используется для предотвращения ошибок связанных с отсутствием значения. А также это удобный способ представить отсутствие значения. В отличие от **Nullable**, мы точно знаем что объект может быть получен в None форме (null для классов, default для структур). Nullable сообщает нам о возможности отсутствия значений только у структур, в то время как **Option** можно использовать для всех типов объектов.
1) Проверка наличия обекта в его нормальном состоянии: 
```cs
public bool DoThing(Option<Player> player)
{
    // У типа Option проверяется свойство IsSome. В данном случае используется краткая форма записи. 
    // Полная форма - player.IsSome.
    if (player)
    {
        // Некоторые действия.
        return true;
    }
    return false;
}
```
2) Пример использования со структурами:
```cs
public Option<int> GetNumber(...)
{
    if(...)
      return 100;
    else
      // Возвращаем отсутствующее значение. 
      return Option<int>.None;
}
```
Такой подход позволяет сразу уведомить нас о том, что объект может отсутствовать в его нормальном состоянии. Не нужно придумывать специальные коды по типу "-1" или других магических чисел.

3) Пример использования с расширением LINQ:
```cs
public void ListSceneData(string name)
{
    var scene = _scenes.FirstOrNone(x => x.name == name);
    // Краткая форма записи scene.IsNone.
    if(!scene)
        return;
    
    Debug.Log($"Идентификатор сцены - {scene.option.id}");
}
```
Свойство "option" - это экземпляр объекта. Scene в данном случае это и есть Option\<Scene\>.

4) Пример использования в типе:
```cs
public class/struct Data
{
    public Option<string> Name;
    public Option<AnotherData> AnotherData;
}
```
Я использую **Option** везде, где хочу сохранить себе нервы и душевное спокойствие.
### Promise\<T\>
Данный тип полезен в ситуации, когда нам нужно получить некий объект, однако этот объект будет создан только в будущем. И для того чтобы не создавать события-фантомы (OnValueXCreated, OnValueXFailedCreation), можно использовать **Promise** для передачи данных. Кроме того, Promise сработает даже после первого оповещения: все новые подписчики получат экземпляр данных или уведомление об отсутствии данных.
1) Пример иллюстрирующий разницу в использования событий и типа **Promise**:
```cs
// Объект, использующий возможности типа Promise.
public class CoolObject
{
    public Promise<Data> DataPromise;
}

// Стандартный объект без использования Promise
public class DefaultObject
{
    private Data data;
    public event Action<Data> OnDataCreated;
    public event Action<object> OnDataFailed;
    ...
    // И так для каждого отдельного члена объекта.
    // Либо придется подвязывать все на один момент создания, когда все данные будут готовы.
    ...
}
```
2) Пример взаимодействия с объектом типа **Promise**:
```cs
// Объект, использующий тип Promise.
public class CoolObject 
{
    public Promise<Data> DataPromise;
}

// Некоторый метод
private void LogDataFrom(CoolObject coolObj)
{
    // Ожидаем значение, либо оповещение об ошибке в работе объекта.
    coolObj.DataPromise.GetValue(x => LogData(x));
    coolObj.DataPromise.GetFail (y => LogFail(y));
}
```
Для удобства используется Promise\<TValue,int\>, но вы можете использовать Promise\<TValue,TFail\> для определения своих данных об ошибке. 
### Singleton\<T\> и SingletonRaw\<T\>
Эти объекты-синглтоны я реализовал для более удобного взаимодействия с подобными типами объектов. Две формы Singleton и SingletonRaw нужны для определения синглтона типа MonoBehaviour и синглтона типа object (функционально они одинаковы).

Основной целью при реализации были безопасность и стабильность. Безопасность заключается в возможности указать поведение lazy instance и обработать момент создания экземпляра. Стабильность осуществляется путем настройки поведения и обработки ситуаций существования и удаления экземпляра. 
1) Пример объявления **Singleton**:
```cs
// Синглтон типа MonoBehaviour (unity)
[LazyInstance(false)]
public class GameManager : Singleton<GameManager>
{
    protected override bool CanBeDestroyedOutside => false
}

// Синглтон типа object (.net).
public class RawGameManager : SingletonRaw<GameManager> {...}
```
LazyInstance(false) атрибут говорит о том, что данный объект не может быть создан извне, например при обращении к GameManager.Instance (в большинстве других реализаций, при обращении к свойству экземпляра синглтон сразу же будет создан).

2) Пример получения экземпляра:
```cs
public void DoSome()
{
    // Проверяем существование синглтона.
    if(GameManager.Exist)
        return;
}

public void DoAnother()
{
    // Вместо проверок на существование синглтона,
    // Мы сразу обращаемся к Promise за экземпляром синглтона.
    GameManager.InstancePromise.GetValue(x => DoWithGameManager(x))
}

public void DoUnsafe()
{
    // Напрямую получаем экземпляр синглтона. 
    // Это лучше делать с реально вечными объектами, которые 100% уже существуют в памяти игры.
    var instance = GameManager.Instance;
}
```
3) Пример создания и уничтожения экземпляра:
```cs
// Удаление экземпляра, если он существует, 
// С параметром immediate = true, означающим мгновенное удаление со сцены.
public void DestroySingleton() => SomeSingleton.DestroyIfExist(immediate = true);

// Простое создание экземпляра.
public void CreateSingleton () => SomeSingleton.CreateInstance();
```

## Лицензия
Используется MIT License.

## Контакты
Kirill Gasanov - gasanov.kirill.dev@gmail.com
