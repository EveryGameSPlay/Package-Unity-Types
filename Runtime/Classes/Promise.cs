using System;

namespace Egsp.Core
{
    /// <summary>
    /// <para>Данный объект "обещание" позволяет получить данные, которые будут лишь в будущем.</para>
    /// <para>Если возникнет ошибка, то "обещание" вернет объект ошибки.</para>
    /// 
    /// <para>После вызова одного из событий, все подписчики будут отписаны.</para>
    /// <para>Новые подписчики получат результат, даже если ссобытие было вызвано ранее.</para>
    /// </summary>
    public class Promise<TValue, TFail>
    {
        public enum PromiseState
        {
            Waiting,
            Value,
            Fail
        }
        
        // Одно из полей будет пусто. Option используется для обозначения пустоты, в частности структур.
        private Option<TValue> _value = Option<TValue>.None;
        private Option<TFail> _fail = Option<TFail>.None;
        
        public PromiseState State { get; protected set; } = PromiseState.Waiting;
        
        protected event Action<TValue> OnValue = delegate(TValue obj) {  };
        protected event Action<TFail> OnFail = delegate(TFail obj) {  };

        public TValue Value
        {
            set
            {
                if(State == PromiseState.Fail)
                    throw new InvalidOperationException();
                
                _value = value;
                State = PromiseState.Value;
                
                OnValue(_value.option);
                
                ClearAllSubscribers();
            }
        }

        public TFail Fail
        {
            set
            {
                if (State == PromiseState.Value)
                    throw new InvalidOperationException();
                
                _fail = value;
                State = PromiseState.Fail;
                
                OnFail(_fail.option);
                
                ClearAllSubscribers();
            }
        }
        
        public void GetValue(Action<TValue> resultAction)
        {
            OnValue += resultAction;

            if (_value)
                InvokeValue(_value.option);
        }

        public void GetFail(Action<TFail> failAction)
        {
            OnFail += failAction;

            if (_fail)
                InvokeFail(_fail.option);
        }
        
        protected void InvokeValue(in TValue result)
        {
            OnValue(result);
            ClearAllSubscribers();
        }

        protected void InvokeFail(in TFail fail)
        {
            OnFail(fail);
            ClearAllSubscribers();
        }

        protected void ClearAllSubscribers()
        {
            OnValue = delegate(TValue r) {  };
            OnFail = delegate(TFail f) {  };
        }
    }

    public class Promise<TValue> : Promise<TValue, int>
    {
        
    }
}