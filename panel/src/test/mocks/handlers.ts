import { http, HttpResponse } from 'msw';
import { API_BASE_URL } from '@/lib/constants';

export const handlers = [
  // Mock kierowcy POST
  http.post(`${API_BASE_URL}/kierowcy`, async ({ request }) => {
    const body = await request.json() as any;

    console.log('[MSW] Received kierowca data:', body);

    // Validate required fields
    if (!body.imie || !body.nazwisko || !body.nrPracownika || !body.dataZatrudnienia) {
      return HttpResponse.json(
        {
          type: 'ValidationError',
          title: 'One or more validation errors occurred.',
          status: 400,
          errors: {
            ...(! body.imie && { Imie: ['The Imie field is required.'] }),
            ...(!body.nazwisko && { Nazwisko: ['The Nazwisko field is required.'] }),
            ...(!body.nrPracownika && { NrPracownika: ['The NrPracownika field is required.'] }),
            ...(!body.dataZatrudnienia && { DataZatrudnienia: ['The DataZatrudnienia field is required.'] }),
          },
        },
        { status: 400 }
      );
    }

    // Validate date format
    const dateRegex = /^\d{4}-\d{2}-\d{2}$/;
    if (!dateRegex.test(body.dataZatrudnienia)) {
      return HttpResponse.json(
        {
          type: 'ValidationError',
          title: 'Invalid date format',
          status: 400,
          errors: {
            DataZatrudnienia: ['The date must be in YYYY-MM-DD format.'],
          },
        },
        { status: 400 }
      );
    }

    // Success response
    return HttpResponse.json(1, { status: 201 });
  }),

  // Mock kierowcy GET
  http.get(`${API_BASE_URL}/kierowcy`, () => {
    return HttpResponse.json({
      items: [],
      pageNumber: 1,
      totalPages: 1,
      totalCount: 0,
      hasPreviousPage: false,
      hasNextPage: false,
    });
  }),
];
