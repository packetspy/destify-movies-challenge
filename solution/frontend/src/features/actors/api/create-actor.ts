import { useMutation, useQueryClient } from '@tanstack/react-query'
import { z } from 'zod'

import { api } from '@/lib/api-client'
import { MutationConfig } from '@/lib/react-query'
import { Actor } from '@/types/api'

import { getActorsQueryOptions } from './get-actors'

export const createActorInputSchema = z.object({
  name: z.string().min(1, 'Required'),
  movies: z.any()
})

export type CreateActorInput = z.infer<typeof createActorInputSchema>

export const createActor = ({ data }: { data: CreateActorInput }): Promise<Actor> => {
  return api.post(`/actors`, data)
}

type UseCreateActorOptions = {
  mutationConfig?: MutationConfig<typeof createActor>
}

export const useCreateActor = ({ mutationConfig }: UseCreateActorOptions = {}) => {
  const queryClient = useQueryClient()

  const { onSuccess, ...restConfig } = mutationConfig || {}

  return useMutation({
    onSuccess: (...args) => {
      queryClient.invalidateQueries({
        queryKey: getActorsQueryOptions().queryKey
      })
      onSuccess?.(...args)
    },
    ...restConfig,
    mutationFn: createActor
  })
}
