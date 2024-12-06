import { useMutation, useQueryClient } from '@tanstack/react-query'
import { z } from 'zod'

import { api } from '@/lib/api-client'
import { MutationConfig } from '@/lib/react-query'
import { Movie } from '@/types/api'
import { getMovieQueryOptions } from './get-movie'

export const updateMovieInputSchema = z.object({
  uniqueId: z.string(),
  title: z.string().min(1, 'Required'),
  year: z
    .number()
    .min(1900, 'Invalid year')
    .max(new Date().getFullYear() + 1, 'Invalid year'),
  rated: z.string().min(1, 'Required'),
  genre: z.string().min(1, 'Required'),
  language: z.string().min(1, 'Required'),
  country: z.string().min(1, 'Required'),
  poster: z.string(),
  plot: z.string().min(1, 'Required')
})

export const updateRatingInputSchema = z.object({
  source: z.string().min(1, 'Required'),
  value: z.string().min(1, 'Required')
})

export type UpdateMovieInput = z.infer<typeof updateMovieInputSchema>

export const updateMovie = ({
  data,
  movieUniqueId
}: {
  data: UpdateMovieInput
  movieUniqueId: string
}): Promise<Movie> => {
  return api.put(`/movies/${movieUniqueId}`, data)
}

type UseUpdateMovieOptions = {
  mutationConfig?: MutationConfig<typeof updateMovie>
}

export const useUpdateMovie = ({ mutationConfig }: UseUpdateMovieOptions = {}) => {
  const queryClient = useQueryClient()

  const { onSuccess, ...restConfig } = mutationConfig || {}

  return useMutation({
    onSuccess: (data, ...args) => {
      queryClient.refetchQueries({
        queryKey: getMovieQueryOptions(data.uniqueId).queryKey
      })
      onSuccess?.(data, ...args)
    },
    ...restConfig,
    mutationFn: updateMovie
  })
}
