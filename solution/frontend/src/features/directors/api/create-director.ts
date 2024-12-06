import { useMutation, useQueryClient } from '@tanstack/react-query'
import { z } from 'zod'

import { api } from '@/lib/api-client'
import { MutationConfig } from '@/lib/react-query'
import { Director } from '@/types/api'

import { getDirectorsQueryOptions } from './get-directors'

export const createDirectorInputSchema = z.object({
  name: z.string().min(1, 'Required'),
  movies: z.any()
})

export type CreateDirectorInput = z.infer<typeof createDirectorInputSchema>

export const createDirector = ({ data }: { data: CreateDirectorInput }): Promise<Director> => {
  return api.post(`/directors`, data)
}

type UseCreateDirectorOptions = {
  mutationConfig?: MutationConfig<typeof createDirector>
}

export const useCreateDirector = ({ mutationConfig }: UseCreateDirectorOptions = {}) => {
  const queryClient = useQueryClient()

  const { onSuccess, ...restConfig } = mutationConfig || {}

  return useMutation({
    onSuccess: (...args) => {
      queryClient.invalidateQueries({
        queryKey: getDirectorsQueryOptions().queryKey
      })
      onSuccess?.(...args)
    },
    ...restConfig,
    mutationFn: createDirector
  })
}
