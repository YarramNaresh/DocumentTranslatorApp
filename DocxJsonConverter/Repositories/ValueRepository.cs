using System;
using System.Collections.Generic;
using System.Text;

namespace DocxJsonConverter.Repositories
{
    public class ValueRepository
    {
        public static string JsonSchema = @"{
              'definitions': {},
              '$schema': 'http://json-schema.org/draft-03/schema#',
              'type': 'object',
              'properties': {
                'id': {
                  'type': 'string',
                  'required': true
                },
                'timestamp': {
                  'type': 'integer',
                  'required': true
                },
                'extractedFrom': {
                  'type': 'string',
                  'required': true},
                'lines': {
                  'type': 'array',
                  'required': true,
                  'items': {
                    'type': 'string'
                  }
                }
              }
            }";
    }
}
