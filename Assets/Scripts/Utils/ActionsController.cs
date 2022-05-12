using System;

namespace Utils
{
    public class ActionsController
    {
        public void CallOnlyOnce(Action action)
        {
            var context = new ContextCallOnlyOnce();

            if (false == context.AlreadyCalled)
            {
                action.Invoke();

                context.AlreadyCalled = true;
            }

        }
    }
    public class ContextCallOnlyOnce
    {
        public bool AlreadyCalled = false;
    }
}