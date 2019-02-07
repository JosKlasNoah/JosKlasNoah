using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Custom.StoryEventManager
{
    public class StoryEventManager
    {
        static StoryEventManager instance;
        Queue<StoryEvent_SO> _storyEventQue = new Queue<StoryEvent_SO>();

        public StoryEventManager()
        {
            instance = this;
        }

        public static void QueStoryEvents(List<StoryEvent_SO> newEvents)
        {
            instance._storyEventQue.Clear();

            foreach (StoryEvent_SO pStoryEvent in newEvents)
            {
                instance._storyEventQue.Enqueue(pStoryEvent);
            }
        }

        public static void OnFinish(StoryEvent_SO storyEvent)
        {

        }
    }
}
