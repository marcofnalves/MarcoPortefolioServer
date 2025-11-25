namespace MarcoPortefolioServer.Functions.v1.lib.server
{
    public static class ServerInternalEvents
    {
        private static readonly Dictionary<string, Delegate> _events = new();
        private static readonly Dictionary<string, Queue<object[]>> _eventCache = new(); // Armazenando eventos pendentes

        // Registra um evento
        public static void RegisterServerEventInternal(string name, Delegate handler)
        {
            if (_events.ContainsKey(name))
                throw new Exception($"Evento '{name}' já existe.");

            Console.WriteLine($"Registrando evento de servidor: {name}");
            _events[name] = handler;

            if (_eventCache.ContainsKey(name))
            {
                while (_eventCache[name].Count > 0)
                {
                    var args = _eventCache[name].Dequeue();
                    handler.DynamicInvoke(args);
                }
                _eventCache.Remove(name);
            }
        }

        // Aciona um evento
        public static void TriggerServerEventInternal(string name, params object[] args)
        {
            if (!_events.TryGetValue(name, out var handler))
            {
                Console.WriteLine($"Evento '{name}' não encontrado. Armazenando em cache.");

                // Se o evento ainda não foi registrado, armazene os parâmetros no cache
                if (!_eventCache.ContainsKey(name))
                {
                    _eventCache[name] = new Queue<object[]>();
                }

                _eventCache[name].Enqueue(args); // Adiciona o evento ao cache
                return;
            }

            // Quando o evento for registrado, executa imediatamente
            handler.DynamicInvoke(args);
        }

        // Aciona um evento com callback
        public static void TriggerServerCallbackInternal(string name, object args, Action<object> callback)
        {
            if (!_events.TryGetValue(name, out var handler))
            {
                Console.WriteLine($"Evento '{name}' não encontrado.");
                return;
            }

            // Processar o evento
            var result = handler.DynamicInvoke(args);

            // Após o evento, chama o callback com o resultado
            callback(result);
        }
    }
}