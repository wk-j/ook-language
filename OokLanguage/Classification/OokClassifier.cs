﻿//***************************************************************************
//
//    Copyright (c) Microsoft Corporation. All rights reserved.
//    This code is licensed under the Visual Studio SDK license terms.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
//
//***************************************************************************

namespace OokLanguage
{
    using System;
    using System.Collections.Generic;
    using Microsoft.VisualStudio.Text;
    using Microsoft.VisualStudio.Text.Classification;
    using Microsoft.VisualStudio.Text.Tagging;
    using Tag;

    internal sealed class OokClassifier : ITagger<ClassificationTag>
    {
        ITextBuffer _buffer;
        ITagAggregator<OokTokenTag> _aggregator;
        IDictionary<OokTokenTypes, IClassificationType> _ookTypes;

        internal OokClassifier(ITextBuffer buffer, 
                               ITagAggregator<OokTokenTag> ookTagAggregator, 
                               IClassificationTypeRegistryService typeService)
        {
            _buffer = buffer;
            _aggregator = ookTagAggregator;
            _ookTypes = new Dictionary<OokTokenTypes, IClassificationType>();
            _ookTypes[OokTokenTypes.OokExclamation] = typeService.GetClassificationType("ook!");
            _ookTypes[OokTokenTypes.OokPeriod] = typeService.GetClassificationType("ook.");
            _ookTypes[OokTokenTypes.OokQuestion] = typeService.GetClassificationType("ook?");
            _ookTypes[OokTokenTypes.CakeFunction] = typeService.GetClassificationType("cakeFunction");
        }

        public event EventHandler<SnapshotSpanEventArgs> TagsChanged
        {
            add { }
            remove { }
        }

        public IEnumerable<ITagSpan<ClassificationTag>> GetTags(NormalizedSnapshotSpanCollection spans)
        {
            foreach (var tagSpan in _aggregator.GetTags(spans))
            {
                var tagSpans = tagSpan.Span.GetSpans(spans[0].Snapshot);
                yield return 
                    new TagSpan<ClassificationTag>(tagSpans[0], 
                                                   new ClassificationTag(_ookTypes[tagSpan.Tag.type]));
            }
        }
    }
}
