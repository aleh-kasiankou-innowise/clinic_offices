using Innowise.Clinic.Offices.Persistence.Models;
using Innowise.Clinic.Offices.Persistence.Repositories.Interfaces;
using Innowise.Clinic.Offices.Services.Dto;
using Innowise.Clinic.Offices.Services.Dto.Mappings;
using Innowise.Clinic.Offices.Services.Exceptions;
using Innowise.Clinic.Offices.Services.OfficeService.Interfaces;
using Innowise.Clinic.Shared.Constants;
using Innowise.Clinic.Shared.Dto;
using Innowise.Clinic.Shared.Enums;
using Innowise.Clinic.Shared.MassTransit.MessageTypes.Events;
using Innowise.Clinic.Shared.MassTransit.MessageTypes.Requests;
using MassTransit;
using Microsoft.AspNetCore.Http;

namespace Innowise.Clinic.Offices.Services.OfficeService.Implementations;

public class OfficeService : IOfficeService
{
    private readonly IOfficeRepository _officeRepository;
    private readonly IBus _bus;
    private readonly IRequestClient<BlobUploadRequest> _blobUploadClient;
    private readonly IRequestClient<BlobUpdateRequest> _blobUpdateClient;
    private readonly IRequestClient<BlobDeletionRequest> _blobDeletionClient;


    public OfficeService(IBus bus,
        IRequestClient<BlobUploadRequest> blobUploadClient, IOfficeRepository officeRepository,
        IRequestClient<BlobUpdateRequest> blobUpdateClient, IRequestClient<BlobDeletionRequest> blobDeletionClient)
    {
        _bus = bus;
        _blobUploadClient = blobUploadClient;
        _officeRepository = officeRepository;
        _blobUpdateClient = blobUpdateClient;
        _blobDeletionClient = blobDeletionClient;
    }


    public async Task<IEnumerable<OfficeModel>> GetOfficesAsync()
    {
        return await _officeRepository.GetOfficesAsync();
    }

    public async Task<OfficeModel?> GetOfficeAsync(Guid id)
    {
        return await _officeRepository.GetOfficeAsync(id);
    }

    public async Task<Guid> CreateOfficeAsync(OfficeUploadDto officeDto)
    {
        var imageGenerationResult = await HandleImageAsync(officeDto.Image, null, false);
        var newOffice = officeDto.ToModel(imageGenerationResult.ImageUrl);
        await _officeRepository.CreateOfficeAsync(newOffice);

        await _bus.Publish(
            new OfficeUpdatedMessage(OfficeChange.Add,
                new OfficeAddressDto(newOffice.Id, newOffice.OfficeAddress.ToString()))
        );

        if (imageGenerationResult.Exception is not null)
        {
            throw imageGenerationResult.Exception;
        }

        return newOffice.Id;
    }

    public async Task UpdateOfficeAsync(OfficeModel currentOfficeModel, OfficeUpdateDto officeUpdateDto)
    {
        var imageUpdateResult = await HandleImageAsync(officeUpdateDto.NewImage, officeUpdateDto.ImageUrl,
            officeUpdateDto.IsDeleteImage);
        var updatedOffice = officeUpdateDto.ToModel(currentOfficeModel.Id, imageUpdateResult.ImageUrl);
        await _officeRepository.UpdateOfficeAsync(updatedOffice);

        await _bus.Publish(
            new OfficeUpdatedMessage(OfficeChange.Update,
                new OfficeAddressDto(currentOfficeModel.Id, officeUpdateDto.OfficeAddress.ToString()))
        );

        if (imageUpdateResult.Exception is not null)
        {
            throw imageUpdateResult.Exception;

        }
    }

    public async Task DeleteOfficeAsync(OfficeModel officeToDelete)
    {
        var imageDeletionResult = await HandleImageAsync(null, officeToDelete.ImageUrl, true);
        await _officeRepository.DeleteOfficeAsync(officeToDelete);

        await _bus.Publish(
            new OfficeUpdatedMessage(OfficeChange.Remove,
                new OfficeAddressDto(officeToDelete.Id, null))
        );

        if (imageDeletionResult.Exception is not null)
        {
            throw imageDeletionResult.Exception;

        }
    }

    private async Task<(string? ImageUrl, BlobServiceException? Exception)> HandleImageAsync(IFormFile? newImage,
        string? currentImageUrl, bool isDeleteImage)
    {
        try
        {
            if (!isDeleteImage)
            {
                if (currentImageUrl is null && newImage is not null)
                {
                    return (await SaveImageAsync(newImage), null);
                }

                if (currentImageUrl is not null && newImage is not null)
                {
                    return (await UpdateImageAsync(newImage, currentImageUrl), null);
                }

                return (currentImageUrl, null);
            }

            return currentImageUrl is null
                ? (null, null)
                : (await DeleteImageAsync(currentImageUrl), null);
        }
        catch (BlobServiceException e)
        {
            return (currentImageUrl, e);
        }   
    }

    private async Task<string?> DeleteImageAsync(string currentImageUrl)
    {
        var deletionResponse = await _blobDeletionClient.GetResponse<BlobDeletionResponse>(new(currentImageUrl));

        if (deletionResponse.Message.IsSuccessful)
        {
            return null;
        }

        throw new BlobServiceException(
            "The office image cannot be deleted. The office deletion is cancelled. Please try again later or contact our support team.",
            true);
    }

    private async Task<string> SaveImageAsync(IFormFile newImage)
    {
        var fileContent = await ConvertFileToBytes(newImage);
        var imageCreationResponse = await _blobUploadClient
            .GetResponse<BlobUploadResponse>(new(fileContent, newImage.ContentType, BlobCategories.OfficePhoto));


        if (imageCreationResponse.Message is { IsSuccessful: true, FileUrl: { } })
        {
            return imageCreationResponse.Message.FileUrl;
        }
        
        throw new BlobServiceException(
            "The office image cannot be saved. Please try adding image later or contact our support team.");
    }

    private async Task<string> UpdateImageAsync(IFormFile newImage, string currentImageUrl)
    {
        var fileContent = await ConvertFileToBytes(newImage);
        var imageUpdateResponse =
            await _blobUpdateClient.GetResponse<BlobUpdateResponse>(new(fileContent, newImage.ContentType,
                currentImageUrl));

        if (imageUpdateResponse.Message.IsSuccessful)
        {
            return currentImageUrl;
        }

        throw new BlobServiceException(
            "The office image cannot be updated. Please try updating image later or contact our support team.");
    }

    private static async Task<byte[]> ConvertFileToBytes(IFormFile file)
    {
        using var ms = new MemoryStream();
        await file.CopyToAsync(ms);
        return ms.ToArray();
    }
}