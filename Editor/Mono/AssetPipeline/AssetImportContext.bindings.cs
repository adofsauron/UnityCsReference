// Unity C# reference source
// Copyright (c) Unity Technologies. For terms of use, see
// https://unity3d.com/legal/licenses/Unity_Reference_Only_License

using System;
using UnityEngine;
using UnityEngine.Bindings;
using UnityEngine.Scripting;
using Object = UnityEngine.Object;

namespace UnityEditor.Experimental.AssetImporters
{
    /// Universal structure that holds all the data relevant to importing an asset, including temporary data that needs to be shared across stages that make on any given importer's pipeline.
    ///
    /// Breaking up legacy importers into peaces and re-arranging them as pipelines that use those pieces so that the pieces can become building blocks that other importers can re-use implies that the pieces
    /// is not coupled with any given importer. For this decoupling and maximizing reuse, we need something that can hold the information describing what is being imported but also the data generated by the
    /// various parts that make up an importers pipeline. This container simply transports information from one "stage" to the other. Each stage is free to add/delete/alter the content of the container
    [RequiredByNativeCode]
    [NativeHeader("Editor/Src/AssetPipeline/AssetImportContext.h")]
    public class AssetImportContext
    {
        // The bindings generator is setting the instance pointer in this field
        internal IntPtr m_Self;

        // the context can only be instantiated in native code
        private AssetImportContext() {}

        public extern string assetPath { get; internal set; }
        public extern BuildTarget selectedBuildTarget { get; }

        [NativeThrows]
        public extern void SetMainObject(Object obj);

        public void AddObjectToAsset(string identifier, Object obj)
        {
            AddObjectToAsset(identifier, obj, null);
        }

        [NativeThrows]
        public extern void AddObjectToAsset(string identifier, Object obj, Texture2D thumbnail);

        // Create a dependency against the contents of the source asset at the provided path
        // * if the asset at the path changes, it will trigger an import
        // * if the asset at the path moves, it will trigger an import
        internal void DependOnHashOfSourceFile(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException("path", "Cannot add a null path");
            }

            DependOnHashOfSourceFileInternal(path);
        }

        [NativeName("DependOnHashOfSourceFile")]
        private extern void DependOnHashOfSourceFileInternal(string path);
    }
}