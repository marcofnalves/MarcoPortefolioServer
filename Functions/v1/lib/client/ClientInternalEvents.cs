namespace MarcoPortefolioServer.Functions.v1.lib.client
{
    public static class ClientInternalEvents
    {
        private static readonly Dictionary<string, Delegate> _events = new();
        private static readonly Dictionary<string, Queue<object[]>> _eventCache = new(); // Armazenando eventos pendentes

        // Registra um evento
        public static void RegisterClientEventInternal(string name, Action<object[]> handler)
        {
            if (_events.ContainsKey(name))
                throw new Exception($"Evento '{name}' já existe.");

            Console.WriteLine($"Registrando o evento {name} no client");
            _events[name] = handler;

            // Processa eventos armazenados no cache
            if (_eventCache.ContainsKey(name))
            {
                while (_eventCache[name].Count > 0)
                {
                    var args = _eventCache[name].Dequeue(); // Obtém os parâmetros armazenados
                    handler.Invoke(args); // Executa o evento armazenado
                }
                _eventCache.Remove(name); // Limpa o cache após processar os eventos
            }
        }

        // Aciona um evento
        public static void TriggerClientEventInternal(string name, params object[] args)
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

            // Verifica se o handler é do tipo Action
            if (handler is Action<object[]> typedHandler)
            {
                typedHandler.Invoke(args); // Invoca o handler com os parâmetros passados
            }
        }

        // Aciona um evento com callback
        public static void TriggerClientCallbackInternal(string name, object args, Action<object> callback)
        {
            if (!_events.TryGetValue(name, out var handler))
            {
                Console.WriteLine($"Evento '{name}' não encontrado.");
                return;
            }

            // Verifica se o handler é do tipo Action
            if (handler is Action<object[]> typedHandler)
            {
                // Processa o evento sem armazenar o resultado, pois é um Action<void>
                typedHandler.Invoke(new object[] { args });

                // Após o evento, chama o callback com o resultado
                callback(args); // Passando `args` diretamente para o callback
            }
            else
            {
                Console.WriteLine($"Evento '{name}' não corresponde ao tipo de handler esperado.");
            }
        }

        // Aciona uma requisição ao servidor e processa o callback
        public static void TriggerServerCallbackInternal(string name, object args, Action<object> callback)
        {
            if (!_events.TryGetValue(name, out var handler))
            {
                Console.WriteLine($"Evento '{name}' não encontrado.");
                return;
            }

            Console.WriteLine($"Enviando requisição ao servidor para o evento '{name}' com parâmetros: {args}");

            Task.Run(() =>
            {
                // Simula uma requisição ao servidor
                Task.Delay(1000).Wait(); // Simula espera de 1 segundo para resposta do servidor

                // Simulando a resposta do servidor (nesse caso, apenas passando `args` como resposta)
                var result = args;

                // Chama o callback com a resposta
                callback(result);
            });
        }
    }
}
