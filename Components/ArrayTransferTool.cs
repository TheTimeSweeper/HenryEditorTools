using RoR2;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;

namespace HenryTools
{
    public class ArrayTransferTool : MonoBehaviour
    {

        [SerializeField]
        private List<GameObject> objectsToSend;

        [ContextMenu("Send to CharacterModel Rendererinfos")]
        public void sendRendererInfos()
        {

            CharacterModel charaModel = GetComponent<CharacterModel>();

            if (charaModel == null)
            {

                Debug.LogError("no CharacterModel attached");
                return;
            }

#if UNITY_EDITOR
            UnityEditor.Undo.RecordObject(charaModel, "grab rends");
            UnityEditor.Undo.RecordObject(this, "grab rends");
#endif

            charaModel.baseRendererInfos = new CharacterModel.RendererInfo[objectsToSend.Count];
            for (int i = 0; i < objectsToSend.Count; i++)
            {

                Renderer rend = objectsToSend[i].GetComponent<Renderer>();

                charaModel.baseRendererInfos[i] = new CharacterModel.RendererInfo
                {
                    renderer = rend,
                    defaultMaterial = rend.sharedMaterial,
                    defaultShadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On,
                };
            }

            objectsToSend.Clear();
        }

        [ContextMenu("Send to ChildLocator TransformPairs")]
        public void sendChildLocator()
        {

            ChildLocator childLocator = GetComponent<ChildLocator>();

            if (childLocator == null)
            {

                Debug.LogError("no ChildLocator attached");
                return;
            }

#if UNITY_EDITOR
            UnityEditor.Undo.RecordObject(childLocator, "grab transes");
            UnityEditor.Undo.RecordObject(this, "grab rends");
#endif

            int originalLength = childLocator.transformPairs.Length;
            Array.Resize(ref childLocator.transformPairs, childLocator.transformPairs.Length + objectsToSend.Count);
            for (int i = 0; i < objectsToSend.Count; i++)
            {

                childLocator.transformPairs[i + originalLength] = new ChildLocator.NameTransformPair
                {
                    name = objectsToSend[i].name,
                    transform = objectsToSend[i].transform,
                };
            }

            objectsToSend.Clear();
        }
    }
}