namespace Aplicacion.Modelos;

public class RespuestaDto<T>
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; } = string.Empty;
    public T? Data { get; set; }
}
