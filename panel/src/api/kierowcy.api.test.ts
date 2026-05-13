import { describe, it, expect, vi, beforeEach } from 'vitest';
import { kierowcyApi } from './kierowcy.api';
import { apiClient } from './client';
import type { UtworzKierowceDto } from '@/types/kierowca.types';

vi.mock('./client', () => ({
  apiClient: {
    get: vi.fn(),
    post: vi.fn(),
  },
}));

describe('kierowcyApi', () => {
  beforeEach(() => {
    vi.clearAllMocks();
  });

  describe('create', () => {
    it('should send POST request with correct data format', async () => {
      const mockResponse = { data: 1 };
      vi.mocked(apiClient.post).mockResolvedValue(mockResponse);

      const kierowcaData: UtworzKierowceDto = {
        imie: 'Jan',
        nazwisko: 'Kowalski',
        nrPracownika: '12345',
        dataZatrudnienia: '2024-01-15',
      };

      const result = await kierowcyApi.create(kierowcaData);

      expect(apiClient.post).toHaveBeenCalledWith('/kierowcy', kierowcaData);
      expect(result).toBe(1);
    });

    it('should send data with camelCase property names', async () => {
      const mockResponse = { data: 1 };
      vi.mocked(apiClient.post).mockResolvedValue(mockResponse);

      const kierowcaData: UtworzKierowceDto = {
        imie: 'Jan',
        nazwisko: 'Kowalski',
        nrPracownika: '12345',
        dataZatrudnienia: '2024-01-15',
      };

      await kierowcyApi.create(kierowcaData);

      const callArgs = vi.mocked(apiClient.post).mock.calls[0];
      const sentData = callArgs[1] as UtworzKierowceDto;

      // Verify camelCase property names
      expect(sentData).toHaveProperty('imie');
      expect(sentData).toHaveProperty('nazwisko');
      expect(sentData).toHaveProperty('nrPracownika');
      expect(sentData).toHaveProperty('dataZatrudnienia');

      // Verify no PascalCase properties
      expect(sentData).not.toHaveProperty('Imie');
      expect(sentData).not.toHaveProperty('Nazwisko');
      expect(sentData).not.toHaveProperty('NrPracownika');
      expect(sentData).not.toHaveProperty('DataZatrudnienia');
    });

    it('should send date in YYYY-MM-DD format', async () => {
      const mockResponse = { data: 1 };
      vi.mocked(apiClient.post).mockResolvedValue(mockResponse);

      const kierowcaData: UtworzKierowceDto = {
        imie: 'Jan',
        nazwisko: 'Kowalski',
        nrPracownika: '12345',
        dataZatrudnienia: '2024-01-15',
      };

      await kierowcyApi.create(kierowcaData);

      const callArgs = vi.mocked(apiClient.post).mock.calls[0];
      const sentData = callArgs[1] as UtworzKierowceDto;

      // Verify date format
      expect(sentData.dataZatrudnienia).toMatch(/^\d{4}-\d{2}-\d{2}$/);
    });

    it('should handle API errors', async () => {
      const mockError = new Error('Network error');
      vi.mocked(apiClient.post).mockRejectedValue(mockError);

      const kierowcaData: UtworzKierowceDto = {
        imie: 'Jan',
        nazwisko: 'Kowalski',
        nrPracownika: '12345',
        dataZatrudnienia: '2024-01-15',
      };

      await expect(kierowcyApi.create(kierowcaData)).rejects.toThrow('Network error');
    });
  });

  describe('getList', () => {
    it('should send GET request with query params', async () => {
      const mockResponse = {
        data: {
          items: [],
          pageNumber: 1,
          totalPages: 1,
          totalCount: 0,
          hasPreviousPage: false,
          hasNextPage: false,
        },
      };
      vi.mocked(apiClient.get).mockResolvedValue(mockResponse);

      const params = {
        Page: 1,
        PageSize: 10,
        szukaj: 'test',
      };

      const result = await kierowcyApi.getList(params);

      expect(apiClient.get).toHaveBeenCalledWith('/kierowcy', { params });
      expect(result).toEqual(mockResponse.data);
    });
  });
});
