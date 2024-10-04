﻿namespace APIEntidades.Domain.Dto
{
    public class ResponseDto<T>
    {
        public bool Success { get; set; }
        public string? ErrorMessage { get; set; }
        public T Data { get; set; }
    }
}
