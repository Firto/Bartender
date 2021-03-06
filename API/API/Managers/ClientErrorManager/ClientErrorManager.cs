﻿using API.DtoModels;
using API.DtoModels.ClientErrors;
using API.DtoModels.ClientErrors.Info;
using API.Hostings.ServiceInitialization.Abstraction;
using System.Collections.Generic;

namespace API.Managers.CClientErrorManager
{    
    public class ClientErrorManager : ClientErrors, IMyService
    {
        static ClientErrorManager()
            => _errors.Add("inc-err", new ClientError("Incorrect error!"));

        private static ClientError GetErrorByIDSave(string Id)
            => GetErrorByID(Id) ?? _errors["inc-err"];

        public ClientError GetErrorByIDS(string Id)
            => GetErrorByID(Id) ?? _errors["inc-err"];

        private static ClientErrorDto MapClientError(ClientError error, object AdditionalInfo = null) {
            var id = GetId(error);
            return AdditionalInfo == null ? new ClientErrorDto
            {
                Id = id,
                Message = $"[cl-{id}]"
            }:
            new ClientErrorDtoPlus
            {
                Id = id,
                Message = $"[cl-{id}]",
                AdditionalInfo = AdditionalInfo
            };
        }

        public override ClientErrorsInfoDto GetInfo()
        {
            ClientErrorsInfoDto dto = new ClientErrorsInfoDto();
            if (_clientErrors.Count > 0)
            {
                dto.Errors = new Dictionary<string, ClientErrorInfoDto>();
                foreach (var item in _clientErrors)
                    dto.Errors.Add(item, GetErrorByID(item).GetInfo());
            }
            if (_childLists.Count > 0)
            {
                var set = new List<ClientErrorsInfoDto>();
                foreach (var item in _childLists)
                    set.Add(item.GetInfo());
                dto.Dictionaries = set;
            }
            return dto;
        }

        private static ClientErrorDto MapClientError(string Id, object AdditionalInfo = null)
        {
            ClientError error = GetErrorByIDSave(Id);
            return MapClientError(error, AdditionalInfo);
        }

        private static ResultDto MapClientErrorDtoToResultDto(ClientErrorDto dto) 
            => new ResultDto { ok = false, result = dto };

        private static ResultDto MapClientErrorDtoToResultDto(ClientError error, object AdditionalInfo = null)
            => new ResultDto { ok = false, result = MapClientError(error, AdditionalInfo) };

        public ResultDto MapClientErrorDtoToResultDto(string Id, object AdditionalInfo = null)
           => new ResultDto { ok = false, result = MapClientError(Id, AdditionalInfo) };
    }
}