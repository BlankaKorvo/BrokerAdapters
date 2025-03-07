/*
 * Trade API
 *
 * No description provided (generated by Openapi Generator https://github.com/openapitools/openapi-generator)
 *
 * The version of the OpenAPI document: current
 * Generated by: https://github.com/openapitools/openapi-generator.git
 */


using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System.ComponentModel.DataAnnotations;
using OpenAPIDateConverter = Org.OpenAPITools.Client.OpenAPIDateConverter;

namespace Org.OpenAPITools.Model
{
    /// <summary>
    /// Stop price.  Цена стоп-заявки.
    /// </summary>
    [DataContract(Name = "StopPrice")]
    public partial class StopPrice : IValidatableObject
    {

        /// <summary>
        /// Gets or Sets Units
        /// </summary>
        [DataMember(Name = "units", EmitDefaultValue = false)]
        public StopPriceUnits? Units { get; set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="StopPrice" /> class.
        /// </summary>
        /// <param name="value">Value.  Значение цены..</param>
        /// <param name="units">units.</param>
        public StopPrice(double value = default(double), StopPriceUnits? units = default(StopPriceUnits?))
        {
            this.Value = value;
            this.Units = units;
        }

        /// <summary>
        /// Value.  Значение цены.
        /// </summary>
        /// <value>Value.  Значение цены.</value>
        [DataMember(Name = "value", EmitDefaultValue = false)]
        public double Value { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("class StopPrice {\n");
            sb.Append("  Value: ").Append(Value).Append("\n");
            sb.Append("  Units: ").Append(Units).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }

        /// <summary>
        /// Returns the JSON string presentation of the object
        /// </summary>
        /// <returns>JSON string presentation of the object</returns>
        public virtual string ToJson()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this, Newtonsoft.Json.Formatting.Indented);
        }

        /// <summary>
        /// To validate all properties of the instance
        /// </summary>
        /// <param name="validationContext">Validation context</param>
        /// <returns>Validation Result</returns>
        IEnumerable<ValidationResult> IValidatableObject.Validate(ValidationContext validationContext)
        {
            yield break;
        }
    }

}
