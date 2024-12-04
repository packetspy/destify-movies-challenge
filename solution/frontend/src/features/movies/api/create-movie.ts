import { useMutation, useQueryClient } from '@tanstack/react-query';
import { z } from 'zod';

import { api } from '@/lib/api-client';
import { MutationConfig } from '@/lib/react-query';
import { Movie } from '@/types/api';

import { getMoviesQueryOptions } from './get-movies';

export const createMovieInputSchema = z.object({
  title: z.string().min(1, 'Required'),
  year : z.preprocess((a) => parseInt(z.string().parse(a),10), z.number().min(1900, 'Invalid year').max((new Date()).getFullYear()+1, 'Invalid year')),
  rated: z.string().min(1, 'Required'),
  genre: z.string().min(1, 'Required'),
  language: z.string().min(1, 'Required'),
  country: z.string().min(1, 'Required'),
  poster: z.string(),
  plot: z.string().min(1, 'Required'),
});

export type CreateMovieInput = z.infer<typeof createMovieInputSchema>;

export const createMovie = ({
  data,
}: {
  data: CreateMovieInput;
}): Promise<Movie> => {
  return api.post(`/movies`, data);
};

type UseCreateMovieOptions = {
  mutationConfig?: MutationConfig<typeof createMovie>;
};

export const useCreateMovie = ({
  mutationConfig,
}: UseCreateMovieOptions = {}) => {
  const queryClient = useQueryClient();

  const { onSuccess, ...restConfig } = mutationConfig || {};

  return useMutation({
    onSuccess: (...args) => {
      queryClient.invalidateQueries({
        queryKey: getMoviesQueryOptions().queryKey,
      });
      onSuccess?.(...args);
    },
    ...restConfig,
    mutationFn: createMovie,
  });
};
