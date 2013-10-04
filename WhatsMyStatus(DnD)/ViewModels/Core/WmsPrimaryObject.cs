using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;

namespace WhatsMyStatus_DnD_.ViewModels.Core
{
    public class WmsPrimaryObject
    {
        public const string F_Dbseqnum = "dbseqnum";

        private int? dbseqnum;

        /// <summary>
        /// Primary Key of the object
        /// </summary>
        public int Dbseqnum
        {
            get
            {
                return dbseqnum.GetValueOrDefault(-1);
            }
            set
            {
                dbseqnum = value;
            }
        }

        public virtual XElement ToXElement()
        {
            return new XElement(this.GetType().Name, new XElement(F_Dbseqnum, Dbseqnum));
        }

        /// <summary>
        /// Label - allows nodelabel to be used for databinding
        /// </summary>
        public string Label
        {
            get
            {
                return NodeLabel();
            }
        }

        /// <summary>
        /// Reference to self, used for databinding
        /// </summary>
        public WmsPrimaryObject Self
        {
            get
            {
                return this;
            }
        }

        /// <summary>
        /// Set the primary key
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        public void SetDbseqnum<T>() where T : WmsPrimaryObject
        {
            try
            {
                SetDbseqnum(typeof(T));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        
        public void SetDbseqnum(Type t)
        {
            try
            {
                if (Dbseqnum < 1)
                {
                    var list = WmsFakeDb.Database.GetRelatedTable(t).Cast<WmsPrimaryObject>()
                                                                 .Select(x => x.Dbseqnum)
                                                                 .ToList();

                    if (list.Any())
                    {
                        // Only set the dbseqnum if it is not already set.

                        // All we need is the next value, don't re-use dbseqnums
                        Dbseqnum = list.Max() + 1;

                    }
                    else
                    {
                        Dbseqnum = 1;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public override string ToString()
        {
            return NodeLabel();
        }

        public virtual string NodeLabel()
        {
            string returnValue = "";
            try
            {
                    returnValue = this.Dbseqnum.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            return returnValue;
        }

        static public bool IsSubclassOfRawGeneric(Type generic, Type toCheck)
        {
            try
            {
                while (toCheck != typeof(object))
                {
                    var cur = toCheck.IsGenericType ? toCheck.GetGenericTypeDefinition() : toCheck;
                    if (generic == cur)
                    {
                        return true;
                    }
                    toCheck = toCheck.BaseType;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            return false;
        }
    }
}
