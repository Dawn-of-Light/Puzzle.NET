// *
// * Copyright (C) 2005 Roger Johansson : http://www.puzzleframework.com
// *
// * This library is free software; you can redistribute it and/or modify it
// * under the terms of the GNU Lesser General Public License 2.1 or later, as
// * published by the Free Software Foundation. See the included license.txt
// * or http://www.gnu.org/copyleft/lesser.html for details.
// *
// *

using System;
using System.Collections;
using System.Xml;
using Puzzle.NAspect.Framework.Aop;

namespace Puzzle.NAspect.Framework
{
    /// <summary>
    /// Class that deserializes engine configurations from xml
    /// </summary>
    public class ConfigurationDeserializer
    {
        /// <summary>
        /// return a configured <c>IEngine</c> from an xml element.
        /// </summary>
        /// <param name="xmlRoot">xml node to deserialize</param>
        /// <returns>a configured <c>IEngine</c></returns>
        public IEngine Configure(XmlElement xmlRoot)
        {
            Engine engine = new Engine("App.Config");

            XmlElement o = xmlRoot;

            if (o == null)
                return engine;


            foreach (XmlNode settingsNode in o)
            {
                if (settingsNode.Name == "aspect")
                {
                    IList pointcuts = new ArrayList();
                    IList mixins = new ArrayList();

                    string aspectName = settingsNode.Attributes["name"].Value;


                    foreach (XmlNode aspectNode in settingsNode)
                    {
                        if (aspectNode.Name == "pointcut")
                        {
                            IList interceptors = new ArrayList();

                            foreach (XmlNode pointcutNode in aspectNode)
                            {
                                if (pointcutNode.Name == "interceptor")
                                {
                                    string typeString = pointcutNode.Attributes["type"].Value;
                                    Type interceptorType = Type.GetType(typeString);
                                    if (interceptorType == null)
                                        throw new Exception(
                                            string.Format("Interceptor type '{0}' was not found!", typeString));
                                    object interceptor = Activator.CreateInstance(interceptorType);
                                    interceptors.Add(interceptor);
                                }
                            }

                            IPointcut pointcut = null;
                            if (aspectNode.Attributes["target-signature"] != null)
                            {
                                string targetMethodSignature = aspectNode.Attributes["target-signature"].Value;
                                pointcut = new SignaturePointcut(targetMethodSignature, interceptors);
                            }

                            if (aspectNode.Attributes["target-attribute"] != null)
                            {
                                string attributeTypeString = aspectNode.Attributes["target-attribute"].Value;
                                Type attributeType = Type.GetType(attributeTypeString);
                                if (attributeType == null)
                                    throw new Exception(
                                        string.Format("Attribute type '{0}' was not found!", attributeTypeString));

                                pointcut = new AttributePointcut(attributeType, interceptors);
                            }


                            pointcuts.Add(pointcut);
                        }

                        if (aspectNode.Name == "mixin")
                        {
                            string typeString = aspectNode.Attributes["type"].Value;
                            Type mixinType = Type.GetType(typeString);
                            if (mixinType == null)
                                throw new Exception(string.Format("Mixin type '{0}' was not found!", typeString));
                            mixins.Add(mixinType);
                        }
                    }

                    IGenericAspect aspect = null;

                    if (settingsNode.Attributes["target-signature"] != null)
                    {
                        string targetTypeSignature = settingsNode.Attributes["target-signature"].Value;
                        aspect = new SignatureAspect(aspectName, targetTypeSignature, mixins, pointcuts);
                    }

                    if (settingsNode.Attributes["target-attribute"] != null)
                    {
                        string attributeTypeString = settingsNode.Attributes["target-attribute"].Value;
                        Type attributeType = Type.GetType(attributeTypeString);

                        aspect = new AttributeAspect(aspectName, attributeType, mixins, pointcuts);
                    }

                    engine.Configuration.Aspects.Add(aspect);
                }
            }


            return engine;
        }
    }
}