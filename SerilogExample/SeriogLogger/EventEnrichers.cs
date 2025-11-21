using System.Collections.Generic;
using Serilog.Core;
using Serilog.Core.Enrichers;

namespace SerilogExample.SeriogLogger
{
    /// <summary>
    /// Глобальные параметры для фильтрации сообщений при поиске нужных сообщений логов
    /// Например: по имени приложения, по идентификатору пользователя
    /// </summary>
    public class EventEnrichers
    {
       private ILogEventEnricher[] Enrichers { get;  set; }

       private EventEnrichers(Dictionary<string, object> properties)
       {
           Enrichers = new ILogEventEnricher[properties.Count];
           int i = 0;
           foreach (var kv in properties)
           {
               Enrichers[i] = new PropertyEnricher(kv.Key, kv.Value);
               i++;
           }
       }

       /// <summary>
       /// Метод  для заполнения глобальных параметров на базе ключ:значение
       /// </summary>
       /// <param name="externals"></param>
       /// Параметры, которые свойственны данному клинету
       ///
       /// properties - Глобальные параметры, свойственны приложению
       /// <returns>ILogEventEnricher[]</returns>
       public static ILogEventEnricher[] Build(Dictionary<string, object> externals)
       {
           EventEnrichers instance = new EventEnrichers(externals);

           return instance.Enrichers;
       }
    }
}